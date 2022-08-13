using System.Numerics;

namespace RayTracer;

public class HitPoint
{
    public Vector3 Point { get; init; }
    public Vector3 Normal { get; init; }
    public float T { get; init; }
    public bool FrontFace { get; init; }

    public HitPoint(Ray ray, float t, Vector3 outwardNormal)
    {
        Point = ray.PositionAt(t);
        FrontFace = Vector3.Dot(ray.Direction, outwardNormal) < 0;
        Normal = FrontFace ? outwardNormal : -outwardNormal;
    }
}