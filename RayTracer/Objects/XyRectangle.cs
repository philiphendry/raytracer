using System.Numerics;
using RayTracer.Materials;

namespace RayTracer.Objects;

public class XyRectangle : IHittable
{
    private readonly float _x0;
    private readonly float _x1;
    private readonly float _y0;
    private readonly float _y1;
    private readonly float _k;
    private readonly IMaterial _material;
    private readonly AxisAlignedBoundingBox _boundingBox;

    public XyRectangle(float x0, float x1, float y0, float y1, float k, IMaterial material)
    {
        _x0 = x0;
        _x1 = x1;
        _y0 = y0;
        _y1 = y1;
        _k = k;
        _material = material;
        _boundingBox = new(new Vector3(_x0, _y0, _k - 0.0001f), new Vector3(_x1, _y1, _k + 0.0001f));
    }

    public HitPoint? Hit(Ray ray, float tMin, float tMax)
    {
        var t = (_k - ray.Origin.Z) / ray.Direction.Z;
        if (t < tMin || t > tMax)
            return null;

        var x = ray.Origin.X + t * ray.Direction.X;
        var y = ray.Origin.Y + t * ray.Direction.Y;
        if (x < _x0 || x > _x1 || y < _y0 || y > _y1)
            return null;

        return new HitPoint(
            ray, 
            t, 
            Vector3.UnitZ, 
            _material, 
            u: (x - _x0) / (_x1 - _x0), 
            v: (y - _y0) / (_y1 - _y0));
    }

    public AxisAlignedBoundingBox BoundingBox() => _boundingBox;
}