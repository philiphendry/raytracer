using System.Numerics;
using RayTracer.Textures;
using RayTracer.Utility;

namespace RayTracer.Materials;

public sealed class DielectricMaterial : MaterialBase
{
    private readonly float _indexOfRefraction;

    public DielectricMaterial(float indexOfRefraction) : base(new SolidColourTexture(new Vector3(1.0f, 1.0f, 1.0f)))
    {
        _indexOfRefraction = indexOfRefraction;
    }

    public override (Vector3 attenuation, Ray scatteredRay)? Scatter(Ray ray, HitPoint hitPoint)
    {
        var refractionIndexRatio = hitPoint.IsFrontFace ? 1.0f / _indexOfRefraction : _indexOfRefraction;

        var unitDirection = ray.Direction.Unit();
        var cosTheta = MathF.Min(Vector3.Dot(-unitDirection, hitPoint.Normal), 1.0f);
        var sinTheta = MathF.Sqrt(1.0f - cosTheta * cosTheta);

        var cannotRefract = refractionIndexRatio * sinTheta > 1.0f;
        var refractedDirection = cannotRefract || Reflectance(cosTheta) > Util.Random()
                ? Vector3.Reflect(unitDirection, hitPoint.Normal)
                : Vector3Utility.Refract(unitDirection, hitPoint.Normal, refractionIndexRatio);

        return (attenuation: Texture.Value(hitPoint.U, hitPoint.V, hitPoint.Point), scatteredRay: new Ray(hitPoint.Point, refractedDirection));
    }

    /// <summary>
    /// Schlick's approximation for reflectance
    /// </summary>
    /// <param name="cosine"></param>
    /// <returns></returns>
    private float Reflectance(float cosine)
    {
        var r0 = (1 - _indexOfRefraction) / (1 + _indexOfRefraction);
        r0 *= r0;
        return r0 + (1 - r0) * MathF.Pow(1 - cosine, 5.0f);
    }
}