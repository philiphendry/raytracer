using System.Numerics;
using RayTracer.Materials;

namespace RayTracer.Objects;

public class Box : IHittable
{
    private readonly bool _enableHitCounts;
    private int _hitCount;
    private readonly HittableList _sides;

    public Box(Vector3 boxMin, Vector3 boxMax, IMaterial material, CommandLineOptions options)
    {
        _sides = new HittableList(
            new IHittable[]
            {
                new XyRectangle(boxMin.X, boxMax.X, boxMin.Y, boxMax.Y, boxMax.Z, material, options),
                new XyRectangle(boxMin.X, boxMax.X, boxMin.Y, boxMax.Y, boxMin.Z, material, options),

                new XzRectangle(boxMin.X, boxMax.X, boxMin.Z, boxMax.Z, boxMax.Y, material, options),
                new XzRectangle(boxMin.X, boxMax.X, boxMin.Z, boxMax.Z, boxMin.Y, material, options),

                new YzRectangle(boxMin.Y, boxMax.Y, boxMin.Z, boxMax.Z, boxMax.X, material, options),
                new YzRectangle(boxMin.Y, boxMax.Y, boxMin.Z, boxMax.Z, boxMin.X, material, options)
            }, options);

        _enableHitCounts = options.EnabledHitCounts;
    }

    public HitPoint? Hit(Ray ray, float tMin, float tMax)
    {
        if (_enableHitCounts)
            Interlocked.Increment(ref _hitCount);

        return _sides.Hit(ray, tMin, tMax);
    }

    public AxisAlignedBoundingBox? BoundingBox() => _sides.BoundingBox();

    public long HitCount => _hitCount;

    public void DisplayHitCounts(int depth = 0) => Console.WriteLine($"{new string(' ', depth * 2)}{nameof(Box)} : {_hitCount:n0}");
}