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
    private readonly bool _enabledHitCounts;
    private int _hitCount;

    public YzRectangle(float y0, float y1, float z0, float z1, float k, IMaterial material, CommandLineOptions options)
    {
        _y0 = y0;
        _y1 = y1;
        _z0 = z0;
        _z1 = z1;
        _k = k;
        _material = material;
        _enabledHitCounts = options.EnabledHitCounts;
    }

    public HitPoint? Hit(Ray ray, float tMin, float tMax)
    {
        if (_enabledHitCounts)
            Interlocked.Increment(ref _hitCount);

        var t = (_k - ray.Origin.X) / ray.Direction.X;
        if (t < tMin || t > tMax)
            return null;

        var y = ray.Origin.Y + t * ray.Direction.Y; 
        var z = ray.Origin.Z + t * ray.Direction.Z;
        if (y < _y0 || y > _y1 || z < _z0 || z > _z1)
            return null;

        return new HitPoint(ray, t, Vector3.UnitZ, _material, (y - _y0) / (_y1 - _y0), (z - _z0) / (_z1 - _z0));
    }

    public AxisAlignedBoundingBox BoundingBox() => new(new Vector3(_k - 0.0001f, _y0, _z0),new Vector3(_k + 0.0001f, _y1, _z1));

    public long HitCount => _hitCount;

    public void DisplayHitCounts(int depth = 0) => Console.WriteLine($"{new string(' ', depth * 2)}{nameof(XyRectangle)} : {_hitCount:n0}");
}