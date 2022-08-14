using System.Numerics;

namespace RayTracer;

public static class VectorExtensions
{
    public static Vector3 Unit(this Vector3 vector) => Vector3.Divide(vector, vector.Length());

    public static bool NearZero(this Vector3 vector) => vector.X < float.Epsilon && vector.Y < float.Epsilon && vector.Z < float.Epsilon;
}