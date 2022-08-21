using System.Collections.Immutable;

namespace RayTracer;

public class BoundedVolumeHierarchyNode : IHittable
{
    private readonly IHittable _hittableLeft;
    private readonly IHittable _hittableRight;
    private readonly AxisAlignedBoundingBox _boundingBox;
    private int _hitCount;
    private static bool _enabledHitCounts;

    public BoundedVolumeHierarchyNode(ImmutableArray<IHittable> objects, CommandLineOptions options)
    {
        _enabledHitCounts = options.EnabledHitCounts;
        var randomAxis = (int)Utility.Random(0, 2);
        Comparison<IHittable> comparator = (a, b) => a.BoundingBox()!.Minimum.GetAxisByIndex(randomAxis) < b.BoundingBox()!.Minimum.GetAxisByIndex(randomAxis) ? -1 : 1;

        switch (objects.Length)
        {
            case 1:
                _hittableLeft = _hittableRight = objects[0];
                break;
            case 2:
                if (comparator.Invoke(objects[0], objects[1]) < 0)
                {
                    _hittableLeft = objects[0];
                    _hittableRight = objects[1];
                }
                else
                {
                    _hittableLeft = objects[1];
                    _hittableRight = objects[0];
                }
                break;
            default:
                var sortedObjects = objects.ToImmutableArray().Sort(comparator);
                var midPointIndex = sortedObjects.Length / 2;
                _hittableLeft = new BoundedVolumeHierarchyNode(sortedObjects.Take(midPointIndex).ToImmutableArray(), options);
                _hittableRight = new BoundedVolumeHierarchyNode(sortedObjects.Skip(midPointIndex).Take(sortedObjects.Length - midPointIndex).ToImmutableArray(), options);
                break;
        }

        var boxLeft = _hittableLeft.BoundingBox();
        var boxRight = _hittableRight.BoundingBox();

        if (boxLeft == null || boxRight == null)
            throw new InvalidOperationException("Something went wrong!");

        _boundingBox = AxisAlignedBoundingBox.CreateSurrounding(boxLeft, boxRight);
    }

    public IHittable HittableLeft => _hittableLeft;

    public IHittable HittableRight => _hittableRight;

    public HitPoint? Hit(Ray ray, float tMin, float tMax)
    {
        if (_enabledHitCounts)
            Interlocked.Increment(ref _hitCount);

        if (!_boundingBox.Hit(ray, tMin, tMax))
            return null;

        var hitPointLeft = _hittableLeft.Hit(ray, tMin, tMax);

        // Only find hits for values of T closer than any match from the left set
        var hitPointRight = _hittableRight.Hit(ray, tMin, hitPointLeft?.T ?? tMax);

        // If the right hit point isn't null then it was closer than the left
        return hitPointRight ?? hitPointLeft;
    }

    public AxisAlignedBoundingBox BoundingBox() => _boundingBox;
    
    public long HitCount => _hitCount;

    public override string ToString()
    {
        return $"({_boundingBox.Minimum.X:n},{_boundingBox.Minimum.Y:n},{_boundingBox.Minimum.Z:n})" +
               $" - ({_boundingBox.Maximum.X:n},{_boundingBox.Maximum.Y:n},{_boundingBox.Maximum.Z:n})" +
               $"{(_enabledHitCounts && _hitCount > 0 ? $", HitCount={_hitCount:n0}" : string.Empty)}";
    }
}

