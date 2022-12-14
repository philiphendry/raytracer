using System.Numerics;
using System.Text.RegularExpressions;

namespace RayTracer.Utility;

public static class Vector3Utility
{
    public static Regex VectorRegex = new (@"^(?<x>-?\d+(\.\d+)?),(?<y>-?\d+(\.\d+)?),(?<z>-?\d+(\.\d+)?)$", RegexOptions.Compiled);

    public static Vector3 Random() => new(Util.Random(), Util.Random(), Util.Random());

    public static Vector3 Random(float min, float max) => new(Util.Random(min, max), Util.Random(min, max), Util.Random(min, max));

    public static Vector3 RandomPointInAUnitSphere()
    {
        while (true)
        {
            var p = Random(-1.0f, 1.0f);
            if (p.LengthSquared() >= 1)
                continue;
            return p;
        }
    }

    public static Vector3 RandomUnitVector() => RandomPointInAUnitSphere().Unit();

    public static Vector3 RandomInHemisphere(Vector3 normal)
    {
        var randomPointInAUnitSphere = RandomPointInAUnitSphere();
        return Vector3.Dot(randomPointInAUnitSphere, normal) > 0.0f
            ? randomPointInAUnitSphere
            : -randomPointInAUnitSphere;
    }

    public static Vector3 RandomInUnitDisk()
    {
        while (true)
        {
            var point = new Vector3(Util.Random(-1.0f, 1.0f), Util.Random(-1.0f, 1.0f), 0);
            if (point.LengthSquared() >= 1)
                continue;
            return point;
        }
    }

    public static Vector3 Reflect(Vector3 vector, Vector3 normal)
        => vector - 2 * Vector3.Dot(vector, normal) * normal;

    public static Vector3 FromString(string? vector)
    {
        var matches = VectorRegex.Match(vector!);
        return new Vector3(float.Parse(matches.Groups["x"].Value), float.Parse(matches.Groups["y"].Value), float.Parse(matches.Groups["z"].Value));
    }

    /// <summary>
    /// Returns a refracted ray solved according to Snell's law :
    ///
    ///     eta * sin theta = eta` * sin theta`
    ///
    /// Where eta and eta` are the refractive indices of the two materials such that,
    /// for example, air is 1.0, glass = 1.3-1.7 and diamond = 2.4.
    ///
    /// theta and theta` is the angle to the normal of the incoming and refracted
    /// ray respectively.
    /// 
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="normal"></param>
    /// <param name="refractionIndexRatio"></param>
    /// <returns></returns>
    public static Vector3 Refract(Vector3 vector, Vector3 normal, float refractionIndexRatio)
    {
        var cosTheta = Math.Min(Vector3.Dot(-vector, normal), 1.0f);
        var rayOutPerpendicular = refractionIndexRatio * (vector + Vector3.Multiply(normal, cosTheta));
        var rayOutParallel = Vector3.Multiply(normal, -MathF.Sqrt(1.0f - rayOutPerpendicular.LengthSquared()));
        return rayOutPerpendicular + rayOutParallel;
    }
}