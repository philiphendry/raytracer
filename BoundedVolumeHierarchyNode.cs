using System.Collections.Immutable;

namespace RayTracer;

public class BoundedVolumeHierarchyNode : IHittable
{
    private readonly IHittable _hittableLeft;
    private readonly IHittable? _hittableRight;
    private readonly AxisAlignedBoundingBox _boundingBox;
    private int _hitCount;
    private static bool _enabledHitCounts;

    public BoundedVolumeHierarchyNode(ImmutableArray<IHittable> objects, CommandLineOptions options)
    {
        _enabledHitCounts = options.EnabledHitCounts;
        var randomAxis = (int)Utility.Random(0, 2);
        
        int Comparator(IHittable a, IHittable b) 
            => a.BoundingBox()!.Minimum.GetAxisByIndex(randomAxis) < b.BoundingBox()!.Minimum.GetAxisByIndex(randomAxis) ? -1 : 1;

        switch (objects.Length)
        {
            case 1:
                _hittableLeft = objects[0];
                _hittableRight = null;
                break;
            case 2:
                if (Comparator(objects[0], objects[1]) < 0)
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
                var sortedObjects = objects.ToImmutableArray().Sort(Comparator).ToArray();
                var midPointIndex = sortedObjects.Length / 2;
                _hittableLeft = new BoundedVolumeHierarchyNode(sortedObjects.Take(midPointIndex).ToImmutableArray(), options);
                _hittableRight = new BoundedVolumeHierarchyNode(sortedObjects.Skip(midPointIndex).Take(sortedObjects.Length - midPointIndex).ToImmutableArray(), options);
                break;
        }

        _boundingBox = (_hittableRight == null 
            ? _hittableLeft.BoundingBox() 
            : AxisAlignedBoundingBox.CreateSurrounding(_hittableLeft.BoundingBox()!, _hittableRight.BoundingBox()!))!;
    }

    public IHittable HittableLeft => _hittableLeft;

    public IHittable? HittableRight => _hittableRight;

    public HitPoint? Hit(Ray ray, float tMin, float tMax)
    {
        if (_enabledHitCounts)
            Interlocked.Increment(ref _hitCount);

        if (!_boundingBox.Hit(ray, tMin, tMax))
            return null;

        var hitPointLeft = _hittableLeft.Hit(ray, tMin, tMax);

        // Only find hits for values of T closer than any match from the left set
        var hitPointRight = _hittableRight?.Hit(ray, tMin, hitPointLeft?.T ?? tMax);

        // If the right hit point isn't null then it was closer than the left
        return hitPointRight ?? hitPointLeft;
    }

    public AxisAlignedBoundingBox BoundingBox() => _boundingBox;
    
    public long HitCount => _hitCount;

    public override string ToString() 
        => $"{_boundingBox}{(_enabledHitCounts && _hitCount > 0 ? $", HitCount={_hitCount:n0}" : string.Empty)}";
}

