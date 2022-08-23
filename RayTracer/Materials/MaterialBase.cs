using RayTracer.Textures;
using System.Numerics;

namespace RayTracer.Materials;

public abstract class MaterialBase : IMaterial
{
    protected readonly Texture Texture;

    protected MaterialBase(Texture texture)
    {
        Texture = texture;
    }

    public abstract (Vector3 attenuation, Ray scatteredRay)? Scatter(Ray ray, HitPoint hitPoint);
}