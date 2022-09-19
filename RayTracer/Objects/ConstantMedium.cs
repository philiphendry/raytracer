using System.Numerics;
using RayTracer.Materials;
using RayTracer.Utility;

namespace RayTracer.Objects;

public sealed class ConstantMedium : IHittable
{
    private readonly IHittable _hittable;
    private readonly IMaterial _material;
    private readonly float _negativeInverseDensity;

    public ConstantMedium(IHittable hittable, float density, Vector3 colour)
    {
        _hittable = hittable;
        _material = new IsotropicMaterial(colour);
        _negativeInverseDensity = -1.0f / density;
    }

    public HitPoint? Hit(Ray ray, float tMin, float tMax)
    {
        var hitPoint1 = _hittable.Hit(ray, float.NegativeInfinity, float.PositiveInfinity);
        if (!hitPoint1.HasValue)
            return null;

        var hitPoint2 = _hittable.Hit(ray, hitPoint1.Value.T + 0.0001f, float.PositiveInfinity);
        if (!hitPoint2.HasValue)
            return null;

        var hitPoint1T = hitPoint1.Value.T;
        if (hitPoint1T < tMin)
            hitPoint1T = tMin;

        var hitPoint2T = hitPoint2.Value.T;
        if (hitPoint2T > tMax)
            hitPoint2T = tMax;

        if (hitPoint1T >= hitPoint2T)
            return null;

        if (hitPoint1T < 0)
            hitPoint1T = 0;

        var rayLength = ray.Direction.Length();
        var distanceInsideBoundary = (hitPoint2T - hitPoint1T) * rayLength;
        var hitDistance = _negativeInverseDensity * MathF.Log(Util.Random());

        if (hitDistance > distanceInsideBoundary)
            return null;

        var t = hitPoint1T + hitDistance / rayLength;
        return new HitPoint(
            ray,
            ray.PositionAt(t),
            t,
            Vector3.UnitX, // arbitrary
            _material,
            0.0f,
            0.0f);
    }

    public AxisAlignedBoundingBox? BoundingBox() => _hittable.BoundingBox();
}