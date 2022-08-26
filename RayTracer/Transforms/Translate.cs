using System.Numerics;

namespace RayTracer.Transforms;

public class Translate : IHittable
{
    private readonly IHittable _obj;
    private readonly Vector3 _offset;

    public Translate(IHittable obj, Vector3 offset)
    {
        _obj = obj;
        _offset = offset;
    }

    public HitPoint? Hit(Ray ray, float tMin, float tMax)
    {
        var movedRay = new Ray(ray.Origin - _offset, ray.Direction);
        var hitPoint = _obj.Hit(movedRay, tMin, tMax);
        if (hitPoint == null)
            return null;

        return new HitPoint(
            movedRay,
            hitPoint.Value.Point + _offset, 
            hitPoint.Value.T, 
            hitPoint.Value.Normal, 
            hitPoint.Value.Material, 
            hitPoint.Value.U, 
            hitPoint.Value.V);
    }

    public AxisAlignedBoundingBox? BoundingBox()
    {
        var boundingBox = _obj.BoundingBox();
        if (boundingBox == null)
            return null;

        return new AxisAlignedBoundingBox(boundingBox.Minimum + _offset, boundingBox.Maximum + _offset);
    }
}