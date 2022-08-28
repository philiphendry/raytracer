using System.Numerics;
using RayTracer.Materials;

namespace RayTracer.Objects;

public class YzRectangle : IHittable
{
    private readonly float _y0;
    private readonly float _y1;
    private readonly float _z0;
    private readonly float _z1;
    private readonly float _k;
    private readonly IMaterial _material;
    private readonly AxisAlignedBoundingBox _boundingBox;

    public YzRectangle(float y0, float y1, float z0, float z1, float k, IMaterial material)
    {
        _y0 = y0;
        _y1 = y1;
        _z0 = z0;
        _z1 = z1;
        _k = k;
        _material = material;
        _boundingBox = new(new Vector3(_k - 0.0001f, _y0, _z0),new Vector3(_k + 0.0001f, _y1, _z1));
    }

    public HitPoint? Hit(Ray ray, float tMin, float tMax)
    {
        var t = (_k - ray.Origin.X) / ray.Direction.X;
        if (t < tMin || t > tMax)
            return null;

        var y = ray.Origin.Y + t * ray.Direction.Y; 
        var z = ray.Origin.Z + t * ray.Direction.Z;
        if (y < _y0 || y > _y1 || z < _z0 || z > _z1)
            return null;

        return new HitPoint(
            ray,
            ray.PositionAt(t),
            t, 
            Vector3.UnitX, 
            _material, 
            u: (y - _y0) / (_y1 - _y0), 
            v: (z - _z0) / (_z1 - _z0));
    }

    public AxisAlignedBoundingBox BoundingBox() => _boundingBox;
}