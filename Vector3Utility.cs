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

    public static Vector3 RandomUnitVector()
    {
        return RandomPointInAUnitSphere().Unit();
    }
}