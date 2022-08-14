using System.Numerics;

namespace RayTracer;

public static class Vector3Utility
{
    public static Vector3 Random() => new(Utility.Random(), Utility.Random(), Utility.Random());

    public static Vector3 Random(float min, float max) => new(Utility.Random(min, max), Utility.Random(min, max), Utility.Random(min, max));

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
        => Vector3.Dot(RandomPointInAUnitSphere(), normal) > 0.0f ? RandomPointInAUnitSphere() : -RandomPointInAUnitSphere();

    public static Vector3 Reflect(Vector3 vector, Vector3 normal)
        => vector - 2 * Vector3.Dot(vector, normal) * normal;
}