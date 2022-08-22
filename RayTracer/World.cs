using System.Collections.Immutable;

namespace RayTracer;

public class World : IHittable
{
    private readonly ImmutableArray<IHittable> _worldObjects;
    private int _hitCount;
    private readonly bool _enabledHitCounts;
    private readonly AxisAlignedBoundingBox? _boundingBox;

    public World(ImmutableArray<IHittable> worldObjects, CommandLineOptions options)
    {
        _worldObjects = worldObjects;
        _enabledHitCounts = options.EnabledHitCounts;
        _boundingBox = CalculateBoundingBox(worldObjects);
    }

    public ImmutableArray<IHittable> Objects => _worldObjects;

    public HitPoint? Hit(Ray ray, float tMin, float tMax)
    {
        if (_enabledHitCounts)
            Interlocked.Increment(ref _hitCount);

        HitPoint? hitPoint = null;
        var closestT = tMax;

        foreach (var worldObject in _worldObjects)
        {
            var nextHitPoint = worldObject.Hit(ray, tMin, closestT);
            if (nextHitPoint.HasValue)
            {
                hitPoint = nextHitPoint;
                closestT = nextHitPoint.Value.T;
            }
        }

        return hitPoint;
    }

    /// <summary>
    /// Calculate the largest bounding box that encapsulates all world objects
    /// </summary>
    /// <returns></returns>
    public AxisAlignedBoundingBox? BoundingBox() => _boundingBox;

    private static AxisAlignedBoundingBox? CalculateBoundingBox(ImmutableArray<IHittable> worldObjects)
    {
        if (worldObjects.IsEmpty)
            return null;

        var firstBox = true;
        AxisAlignedBoundingBox? outputBox = null;
        foreach (var worldObject in worldObjects)
        {
            var boundingBox = worldObject.BoundingBox();
            outputBox = firstBox ? boundingBox : AxisAlignedBoundingBox.CreateSurrounding(outputBox!, boundingBox!);
            firstBox = false;
        }

        return outputBox;
    }

    public long HitCount => _hitCount;

    public void DisplayHitCounts(int depth = 0)
    {
        Console.WriteLine($"{new string(' ', depth * 2)}{nameof(World)} : {_hitCount:n0} {_boundingBox}");
        foreach (var worldObject in _worldObjects)
        {
            worldObject.DisplayHitCounts(depth + 1);
        }
    }
}