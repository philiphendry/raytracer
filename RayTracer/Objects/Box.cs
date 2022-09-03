using System.Numerics;
using RayTracer.Materials;

namespace RayTracer.Objects;

public class Box : IHittable
{
    private readonly HittableList _sides;
    private readonly AxisAlignedBoundingBox _boundingBox;

    public Box(Vector3 boxMin, Vector3 boxMax, IMaterial material)
    {
        _sides = new HittableList(
            new IHittable[]
            {
                new XyRectangle(boxMin.X, boxMax.X, boxMin.Y, boxMax.Y, boxMax.Z, material),
                new XyRectangle(boxMin.X, boxMax.X, boxMin.Y, boxMax.Y, boxMin.Z, material),

                new XzRectangle(boxMin.X, boxMax.X, boxMin.Z, boxMax.Z, boxMax.Y, material),
                new XzRectangle(boxMin.X, boxMax.X, boxMin.Z, boxMax.Z, boxMin.Y, material),

                new YzRectangle(boxMin.Y, boxMax.Y, boxMin.Z, boxMax.Z, boxMax.X, material),
                new YzRectangle(boxMin.Y, boxMax.Y, boxMin.Z, boxMax.Z, boxMin.X, material)
            });
        _boundingBox = new AxisAlignedBoundingBox(boxMin, boxMax);
    }

    public HitPoint? Hit(Ray ray, float tMin, float tMax) => _sides.Hit(ray, tMin, tMax);

    public AxisAlignedBoundingBox BoundingBox() => _boundingBox;
}