using System.Numerics;
using RayTracer.Textures;

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