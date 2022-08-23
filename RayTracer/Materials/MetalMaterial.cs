using System.Numerics;
using RayTracer.Textures;
using RayTracer.Utilities;

namespace RayTracer.Materials;

public class MetalMaterial : MaterialBase
{
    private readonly float _fuzziness;

    public MetalMaterial(Vector3 albedo, float fuzziness) : base(new SolidColour(albedo))
    {
        _fuzziness = fuzziness;
    }

    public override (Vector3 attenuation, Ray scatteredRay)? Scatter(Ray ray, HitPoint hitPoint)
    {
        var reflected = Vector3Utility.Reflect(ray.Direction.Unit(), hitPoint.Normal);
        var scatteredRay = new Ray(hitPoint.Point, reflected + _fuzziness * Vector3Utility.RandomPointInAUnitSphere());
        return Vector3.Dot(scatteredRay.Direction, hitPoint.Normal) > 0
            ? (attenuation: Texture.Value(hitPoint.U, hitPoint.V, hitPoint.Point), scatteredRay)
            : null;
    }
}