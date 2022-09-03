using System.Numerics;
using RayTracer.Materials;

namespace RayTracer;

public struct HitPoint
{
    public IMaterial Material { get; }
    public Vector3 Point { get; }
    public Vector3 Normal { get; set; }
    public float T { get; }
    public bool IsFrontFace { get; set; }
    public float U { get; }
    public float V { get; }

    public HitPoint(
        Ray ray, 
        Vector3 point, 
        float t, 
        Vector3 outwardNormal, 
        IMaterial material, 
        float u, 
        float v)
    {
        Point = point;
        T = t;
        Material = material;
        U = u;
        V = v;
        IsFrontFace = Vector3.Dot(ray.Direction, outwardNormal) < 0;
        Normal = IsFrontFace ? outwardNormal : -outwardNormal;
    }
}