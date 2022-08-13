using System.Numerics;

namespace RayTracer;

public static class VectorExtensions
{
    public static Vector3 Unit(this Vector3 vector) => Vector3.Divide(vector, vector.Length());
}