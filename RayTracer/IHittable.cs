namespace RayTracer;

public interface IHittable
{
    HitPoint? Hit(Ray ray, float tMin, float tMax);

    AxisAlignedBoundingBox? BoundingBox();

    public long HitCount { get; }

    void DisplayHitCounts(int depth = 0);
}