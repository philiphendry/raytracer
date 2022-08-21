using System.Collections.Immutable;

namespace RayTracer;

public class World : IHittable
{
    private readonly ImmutableArray<IHittable> _worldObjects;
    private int _hitCount;
    private readonly bool _enabledHitCounts;

    public World(ImmutableArray<IHittable> worldObjects, CommandLineOptions options)
    {
        _worldObjects = worldObjects;
        _enabledHitCounts = options.EnabledHitCounts;
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
            if (nextHitPoint != null)
            {
                hitPoint = nextHitPoint;
                closestT = nextHitPoint.T;
            }
        }

        return hitPoint;
    }

    /// <summary>
    /// Calculate the largest bounding box that encapsulates all world objects
    /// </summary>
    /// <returns></returns>
    public AxisAlignedBoundingBox? BoundingBox()
    {
        if (_worldObjects.IsEmpty)
            return null;

        var firstBox = true;
        AxisAlignedBoundingBox? outputBox = null;
        foreach (var worldObject in _worldObjects)
        {
            var boundingBox = worldObject.BoundingBox();
            outputBox = firstBox ? boundingBox : AxisAlignedBoundingBox.CreateSurrounding(outputBox!, boundingBox!);
            firstBox = false;
        }

        return outputBox;
    }

    public long HitCount => _hitCount;
}