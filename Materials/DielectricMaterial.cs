using System.Numerics;

namespace RayTracer.Materials;

public class DielectricMaterial : MaterialBase
{
    private readonly float _indexOfRefraction;

    public DielectricMaterial(float indexOfRefraction) : base(new Vector3(1.0f, 1.0f, 1.0f))
    {
        _indexOfRefraction = indexOfRefraction;
    }

    public override (Vector3 attenuation, Ray scatteredRay)? Scatter(Ray ray, HitPoint hitPoint)
    {
        var refractionIndexRatio = hitPoint.FrontFace ? 1.0f / _indexOfRefraction : _indexOfRefraction;

        var unitDirection = ray.Direction.Unit();
        var cosTheta = (float)Math.Min(Vector3.Dot(-unitDirection, hitPoint.Normal), 1.0);
        var sinTheta = (float)Math.Sqrt(1.0f - cosTheta * cosTheta);

        var cannotRefract = refractionIndexRatio * sinTheta > 1.0f;
        var refractedDirection = cannotRefract
                ? Vector3.Reflect(unitDirection, hitPoint.Normal)
                : Vector3Utility.Refract(unitDirection, hitPoint.Normal, refractionIndexRatio);

        return (attenuation: Albedo, scatteredRay: new Ray(hitPoint.Point, refractedDirection));
    }
}