using System.Numerics;

namespace RayTracer.Materials;

public interface IMaterial
{
    (Vector3 attenuation, Ray scatteredRay)? Scatter(Ray ray, HitPoint hitPoint);

    Vector3 Emitted(float u, float v, Vector3 point);
}