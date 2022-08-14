using System.Numerics;

namespace RayTracer.Materials;

public abstract class MaterialBase : IMaterial
{
    protected readonly Vector3 Albedo;

    protected MaterialBase(Vector3 albedo)
    {
        Albedo = albedo;
    }

    public abstract (Vector3 attenuation, Ray scatteredRay)? Scatter(Ray ray, HitPoint hitPoint);
}