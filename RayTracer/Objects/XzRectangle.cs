using System.Numerics;
using RayTracer.Materials;

namespace RayTracer.Objects;

public class XzRectangle : IHittable
{
    private readonly float _x0;
    private readonly float _x1;
    private readonly float _z0;
    private readonly float _z1;
    private readonly float _k;
    private readonly IMaterial _material;
    private readonly AxisAlignedBoundingBox _boundingBox;

    public XzRectangle(float x0, float x1, float z0, float z1, float k, IMaterial material)
    {
        _x0 = x0;
        _x1 = x1;
        _z0 = z0;
        _z1 = z1;
        _k = k;
        _material = material;
        _boundingBox = new(new Vector3(_x0, _k - 0.0001f, _z0),new Vector3(_x1, _k + 0.0001f, _z1));
    }

    public HitPoint? Hit(Ray ray, float tMin, float tMax)
    {
        var t = (_k - ray.Origin.Y) / ray.Direction.Y;
        if (t < tMin || t > tMax)
            return null;

        var x = ray.Origin.X + t * ray.Direction.X;
        var z = ray.Origin.Z + t * ray.Direction.Z;
        if (x < _x0 || x > _x1 || z < _z0 || z > _z1)
            return null;

        return new HitPoint(
            ray, 
            ray.PositionAt(t),
            t, 
            Vector3.UnitY, 
            _material, 
            u: (x - _x0) / (_x1 - _x0), 
            v: (z - _z0) / (_z1 - _z0));
    }

    public AxisAlignedBoundingBox BoundingBox() => _boundingBox;
}