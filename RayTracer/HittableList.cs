using System.Collections.Immutable;

namespace RayTracer;

public class HittableList : IHittable
{
    private readonly IEnumerable<IHittable> _hittables;
    private readonly AxisAlignedBoundingBox? _boundingBox;

    public HittableList(IEnumerable<IHittable> hittables)
    {
        _hittables = new[] { new BoundedVolumeHierarchyNode(hittables.ToImmutableArray()) }.ToImmutableArray<IHittable>();
        _boundingBox = CalculateBoundingBox();
    }

    public HitPoint? Hit(Ray ray, float tMin, float tMax)
    {
        HitPoint? hitPoint = null;
        var closestT = tMax;

        foreach (var hittable in _hittables)
        {
            var nextHitPoint = hittable.Hit(ray, tMin, closestT);
            if (nextHitPoint.HasValue)
            {
                hitPoint = nextHitPoint;
                closestT = nextHitPoint.Value.T;
            }
        }

        return hitPoint;
    }

    public AxisAlignedBoundingBox? BoundingBox() => _boundingBox;

    private AxisAlignedBoundingBox? CalculateBoundingBox()
    {
        if (!_hittables.Any())
            return null;

        var firstBox = true;
        AxisAlignedBoundingBox? outputBox = null;
        foreach (var hittable in _hittables)
        {
            var boundingBox = hittable.BoundingBox();
            outputBox = firstBox ? boundingBox : AxisAlignedBoundingBox.CreateSurrounding(outputBox!, boundingBox!);
            firstBox = false;
        }

        return outputBox;
    }
}