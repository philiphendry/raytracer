using System.Collections.Immutable;
using RayTracer.Utility;

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
        var randomAxis = Utility.Utility.Random(0, 2);
        
        switch (objects.Length)
        {
            case 1:
                _hittableLeft = objects[0];
                _hittableRight = null;
                break;
            case 2:
                if (objects[0].BoundingBox()!.Minimum.GetAxisByIndex(randomAxis) < objects[1].BoundingBox()!.Minimum.GetAxisByIndex(randomAxis))
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
                var sortedObjects = objects.OrderBy(i => i.BoundingBox()!.Minimum.GetAxisByIndex(randomAxis)).ToArray();
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

        // Only find hits for values of T closer than any match from the left set.
        // If the right hit point isn't null then it was closer than the left set.
        return _hittableRight?.Hit(ray, tMin, hitPointLeft?.T ?? tMax) ?? hitPointLeft;
    }

    public AxisAlignedBoundingBox BoundingBox() => _boundingBox;
    
    public long HitCount => _hitCount;

    public void DisplayHitCounts(int depth = 0)
    {
        Console.WriteLine($"{new string(' ', depth * 2)}BVH : {_hitCount:n0} {_boundingBox}");
        _hittableLeft.DisplayHitCounts(depth + 1);
        _hittableRight?.DisplayHitCounts(depth + 1);
    }

    public override string ToString() 
        => $"{_boundingBox}{(_enabledHitCounts && _hitCount > 0 ? $", HitCount={_hitCount:n0}" : string.Empty)}";
}

