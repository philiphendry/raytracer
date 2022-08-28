using System.Numerics;
using RayTracer.Textures;

namespace RayTracer.Materials;

public class DiffuseLightMaterial : MaterialBase
{
    public DiffuseLightMaterial(Texture texture) : base(texture)
    {
    }

    public DiffuseLightMaterial(Vector3 colour) : this(new SolidColour(colour))
    {
    }

    public override (Vector3 attenuation, Ray scatteredRay)? Scatter(Ray ray, HitPoint hitPoint) 
        => null;

    public override Vector3 Emitted(float u, float v, Vector3 point) 
        => Texture.Value(u, v, point);
}