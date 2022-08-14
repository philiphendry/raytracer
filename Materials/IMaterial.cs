using System.Numerics;

namespace RayTracer.Materials;

public interface IMaterial
{
    (Vector3 attenuation, Ray scatteredRay)? Scatter(Ray ray, HitPoint hitPoint);
}