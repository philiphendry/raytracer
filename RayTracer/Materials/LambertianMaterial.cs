using System.Numerics;

namespace RayTracer.Materials;

public class LambertianMaterial : MaterialBase
{
    public LambertianMaterial(Vector3 albedo) : base(new SolidColour(albedo))
    {
    }

    public override (Vector3 attenuation, Ray scatteredRay)? Scatter(Ray ray, HitPoint hitPoint)
    {
        var scatterDirection = hitPoint.Normal + Vector3Utility.RandomUnitVector();

        if (scatterDirection.NearZero())
            scatterDirection = hitPoint.Normal;

        return (attenuation: Texture.Value(hitPoint.U, hitPoint.V, hitPoint.Point), scatteredRay: new Ray(hitPoint.Point, scatterDirection));
    }
}