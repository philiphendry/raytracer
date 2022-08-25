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
    
    public virtual Vector3 Emitted(float u, float v, Vector3 point) => new(0.0f, 0.0f, 0.0f);
}