using System.Collections.Immutable;
using System.Numerics;

namespace RayTracer;

public class Renderer
{
    private readonly Camera _camera;
    private readonly ImmutableArray<IHittable> _worldObjects;
    private readonly int _samplesPerPixel;
    private readonly int _maxDepth;
    private readonly int _chunkSize;
    private readonly bool _normalMaterial;
    private readonly bool _disableLambertian;
    private readonly bool _disableMaterials;
    private int _completedChunks;

    public Renderer(Camera camera, IEnumerable<IHittable> worldObjects, CommandLineOptions options)
    {
        _camera = camera;
        _worldObjects = worldObjects.ToImmutableArray();
        _samplesPerPixel = options.Samples;
        _maxDepth = options.MaxDepth;
        _chunkSize = options.ChunkSize;
        _normalMaterial = options.NormalMaterial;
        _disableLambertian = options.DisableLambertian;
        _disableMaterials = options.DisableMaterials;
    }

    public async Task RenderAsync(
        Bitmap bitmap, 
        IProgress<RenderProgress> progress,
        CancellationToken cancellationToken)
    {
        var renderTasks = (
            from chunkX in Utility.ChunkedRange(_camera.ImageWidth, _chunkSize)
            from chunkY in Utility.ChunkedRange(_camera.ImageHeight, _chunkSize)
            select (chunkX.Item1, chunkY.Item1, chunkX.Item2, chunkY.Item2)
        ).ToImmutableArray();

        _completedChunks = 0;
        await Parallel.ForEachAsync(
            renderTasks,
            cancellationToken,
            async (chunk, ct) => await RenderChunkAsync(chunk.Item1, chunk.Item2, chunk.Item3, chunk.Item4, bitmap, renderTasks.Length, progress, ct));
    }

    private async Task RenderChunkAsync(
        int startX,
        int startY,
        int endX,
        int endY,
        Bitmap bitmap,
        int totalTaskCount,
        IProgress<RenderProgress> progress,
        CancellationToken cancellationToken)
    {
        var result = await RenderAsync(startX, startY, endX, endY, cancellationToken);
        if (result == null)
            return;

        foreach (var renderedPixel in result)
        {
            // Coordinates in bitmap are zero-based but the rendered is one-based
            lock (bitmap)
            {
                bitmap.SetPixel(
                    renderedPixel.Item1 - 1,
                    _camera.ImageHeight - renderedPixel.Item2,
                    VectorToColour(renderedPixel.Item3, _samplesPerPixel));
            }
        }

        Interlocked.Increment(ref _completedChunks);
        progress.Report(new RenderProgress
        {
            Total = totalTaskCount,
            Completed = _completedChunks
        });
    }

    private async Task<List<(int, int, Vector3)>?> RenderAsync(
        int startX, 
        int startY, 
        int endX, 
        int endY,
        CancellationToken cancellationToken)
    {
        var render = new List<(int, int, Vector3)>();
        for (var y = startY; y <= endY; y++)
        {
            for (var x = startX; x <= endX; x++)
            {
                var rayColour = new Vector3(0.0f, 0.0f, 0.0f);
                for (var s = 0; s < _samplesPerPixel; s++)
                {
                    var u = (x + Utility.Random()) / (_camera.ImageWidth - 1);
                    var v = (y + Utility.Random()) / (_camera.ImageHeight - 1);
                    var ray = _camera.GetRay(u, v);
                    rayColour += RayColour(ray, _worldObjects, _maxDepth);
                }
                render.Add((x, y, rayColour));
            }

            await Task.Yield();

            if (cancellationToken.IsCancellationRequested)
                return null;
        }
        return render;
    }

    public static Color VectorToColour(Vector3 vectorColour, int samplesPerPixel)
    {
        var scale = 1.0f / samplesPerPixel;

        // Gamma correct with a factor of 2 which means raising colour to the power 1/gamma which in this case is the square root
        var r = (float)Math.Sqrt(scale * vectorColour.X);
        var g = (float)Math.Sqrt(scale * vectorColour.Y);
        var b = (float)Math.Sqrt(scale * vectorColour.Z);

        return Color.FromArgb(
            (int)(Math.Clamp(r, 0.0f, 0.999f) * 256), 
            (int)(Math.Clamp(g, 0.0f, 0.999f) * 256),
            (int)(Math.Clamp(b, 0.0f, 0.999f) * 256));
    }

    public Vector3 RayColour(Ray ray, ImmutableArray<IHittable> worldObjects, int depth)
    {
        if (worldObjects == null) throw new ArgumentNullException(nameof(worldObjects));

        // Reached max depth so don't collect any more light
        if (depth <= 0)
            return new Vector3(0.0f, 0.0f, 0.0f);

        HitPoint? hitPoint = null;
        var closestT = float.PositiveInfinity;
        foreach (var hittable in worldObjects)
        {
            var nextHitPoint = hittable.Hit(ray, 0.001f, closestT);
            if (nextHitPoint != null)
            {
                hitPoint = nextHitPoint;
                closestT = nextHitPoint.T;
            }
        }

        if (hitPoint != null)
        {
            if (_normalMaterial)
            {
                // Because we have a unit normal we can convert to a colour
                return Vector3.Multiply(
                    new Vector3(hitPoint.Normal.X + 1.0f, hitPoint.Normal.Y + 1.0f, hitPoint.Normal.Z + 1.0f), 0.5f);
            }

            if (!_disableMaterials)
            {
                var scatterResult = hitPoint.Material.Scatter(ray, hitPoint);
                return scatterResult == null
                    ? new Vector3(0.0f, 0.0f, 0.0f)
                    : scatterResult.Value.attenuation *
                      RayColour(scatterResult.Value.scatteredRay, worldObjects, depth - 1);
            }

            var target = _disableLambertian
                ? hitPoint.Point + Vector3Utility.RandomInHemisphere(hitPoint.Normal)
                : hitPoint.Point + hitPoint.Normal + Vector3Utility.RandomUnitVector();

            return 0.5f * RayColour(new Ray(hitPoint.Point, target - hitPoint.Point), worldObjects, depth - 1);
        }

        // We didn't hit an object in the world so calculate a graded background colour

        // t is in the range -1 to 1 so normalise between 0 to 1
        var t = 0.5f * (ray.Direction.Unit().Y + 1.0f);

        // We haven't hit an object so calculate a graduated sky colour
        var white = new Vector3(1.0f, 1.0f, 1.0f);
        var blue = new Vector3(0.5f, 0.7f, 1.0f);
        return Vector3.Add(Vector3.Multiply(white, 1.0f - t), Vector3.Multiply(blue, t));
    }
}