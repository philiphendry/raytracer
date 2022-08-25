using System.Collections.Immutable;
using System.Diagnostics;
using System.Numerics;
using RayTracer.Objects;
using RayTracer.Utilities;

namespace RayTracer;

public class Renderer
{
    private readonly Camera _camera;
    private readonly World _world;
    private readonly int _samplesPerPixel;
    private readonly int _maxDepth;
    private readonly int _chunkSize;
    private readonly bool _normalMaterial;
    private readonly bool _disableLambertian;
    private readonly bool _disableMaterials;
    private int _completedChunks;
    private readonly int _degreesOfParallelism;
    private readonly bool _enableHitCounts;
    private readonly RayColourer _rayColourer;

    public Renderer(Camera camera, World world, CommandLineOptions options)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));

        _camera = camera ?? throw new ArgumentNullException(nameof(camera));
        _world = world ?? throw new ArgumentNullException(nameof(world));
        _samplesPerPixel = options.Samples;
        _maxDepth = options.MaxDepth;
        _chunkSize = options.ChunkSize;
        _normalMaterial = options.NormalMaterial;
        _disableLambertian = options.DisableLambertian;
        _disableMaterials = options.DisableMaterials;
        _enableHitCounts = options.EnabledHitCounts;
        _degreesOfParallelism = 
             Debugger.IsAttached ? 1 : // Whilst debugging only use one thread which makes stepping through code easier!
             options.Parallelism == 0 
                ? Environment.ProcessorCount 
                : Math.Min(Environment.ProcessorCount, options.Parallelism);
        _rayColourer = new RayColourer(_normalMaterial, _disableMaterials, _disableLambertian);
        Console.WriteLine($"Set parallelism to {_degreesOfParallelism}{(options.DisableBvh ? " and BVH is disabled" : string.Empty)}");
    }

    public async Task RenderAsync(
        Bitmap bitmap, 
        IProgress<RenderProgress> progress,
        CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return;

        var renderTasks = (
            from chunkX in Utility.ChunkedRange(_camera.ImageWidth, _chunkSize)
            from chunkY in Utility.ChunkedRange(_camera.ImageHeight, _chunkSize)
            select (chunkX.Item1, chunkY.Item1, chunkX.Item2, chunkY.Item2)
        ).ToImmutableArray();

        _completedChunks = 0;
        var parallelOptions = new ParallelOptions
        {
            CancellationToken = cancellationToken,
            MaxDegreeOfParallelism =  _degreesOfParallelism
        };

        await Parallel.ForEachAsync(
            renderTasks,
            parallelOptions,
            async (chunk, ct) => await RenderChunkAsync(chunk.Item1, chunk.Item2, chunk.Item3, chunk.Item4, bitmap, renderTasks.Length, progress, ct));

        if (_enableHitCounts)
        {
            Dictionary<string, long> hitCounts = new();
            CollectHitCounts(_world, hitCounts);
            foreach (var hitCount in hitCounts)
            {
                Console.WriteLine($"{hitCount.Key} => {hitCount.Value:n0} hits");
            }

            _world.DisplayHitCounts();
        }
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
                    _camera.ImageHeight - renderedPixel.Item2, renderedPixel.Item3.ToColour(_samplesPerPixel));
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
        if (cancellationToken.IsCancellationRequested)
            return null;

        var startTime = DateTime.Now;
        var render = new List<(int, int, Vector3)>();
        for (var y = startY; y <= endY; y++)
        {
            for (var x = startX; x <= endX; x++)
            {
                var rayColour = new Vector3(0.0f, 0.0f, 0.0f);
                for (var s = 0; s < _samplesPerPixel; s++)
                {
                    // The random value adds jitter to each sample
                    var u = (x + Utility.Random()) / (_camera.ImageWidth - 1);
                    var v = (y + Utility.Random()) / (_camera.ImageHeight - 1);
                    var ray = _camera.GetRay(u, v);
                    rayColour += _rayColourer.RayColour(ray, _world, _maxDepth);
                }
                render.Add((x, y, rayColour));

                if (cancellationToken.IsCancellationRequested)
                    return await Task.FromResult<List<(int, int, Vector3)>?>(null);
            }

            if ((DateTime.Now - startTime).TotalMilliseconds > 250)
            {
                await Task.Yield();
                startTime = DateTime.Now;
            }

        }
        return await Task.FromResult(render);
    }

    private static void CollectHitCounts(IHittable hittable, Dictionary<string, long> hitCounts)
    {
        var name = hittable.GetType().Name;
        if (hitCounts.ContainsKey(name))
        {
            hitCounts[name] += hittable.HitCount;
        }
        else
        {
            hitCounts[name] = hittable.HitCount;
        }

        if (hittable is BoundedVolumeHierarchyNode boundedBox)
        {
            CollectHitCounts(boundedBox.HittableLeft, hitCounts);
            if (boundedBox.HittableRight != null)
            {
                CollectHitCounts(boundedBox.HittableRight, hitCounts);
            }
        }
        else if (hittable is World world)
        {
            foreach (var worldObject in world.Objects)
            {
                CollectHitCounts(worldObject, hitCounts);
            }
        }
    }
}