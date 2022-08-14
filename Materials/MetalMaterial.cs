using System.Numerics;

namespace RayTracer.Materials;

public class MetalMaterial : MaterialBase
{
    public MetalMaterial(Vector3 albedo) : base(albedo)
    {
    }

    public override (Vector3 attenuation, Ray scatteredRay)? Scatter(Ray ray, HitPoint hitPoint)
    {
        var reflected = Vector3Utility.Reflect(ray.Direction.Unit(), hitPoint.Normal);
        var scatteredRay = new Ray(hitPoint.Point, reflected);
        return Vector3.Dot(scatteredRay.Direction, hitPoint.Normal) > 0
            ? (attenuation: Albedo, scatteredRay)
            : null;
    }
}