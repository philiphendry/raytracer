using System.Numerics;
using RayTracer.Textures;
using RayTracer.Utility;

namespace RayTracer.Materials;

public class IsotropicMaterial : MaterialBase
{
    public IsotropicMaterial(Texture texture) : base(texture)
    {
    }

    public IsotropicMaterial(Vector3 colour) : base(new SolidColourTexture(colour))
    {
    }

    public override (Vector3 attenuation, Ray scatteredRay)? Scatter(Ray ray, HitPoint hitPoint)
    {
        var scatteredRay = new Ray(hitPoint.Point, Vector3Utility.RandomPointInAUnitSphere());
        var attenuation = Texture.Value(hitPoint.U, hitPoint.V, hitPoint.Point);
        return (attenuation, scatteredRay);
    }
}