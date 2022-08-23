using System.Numerics;

namespace RayTracer.Utilities;

public static class VectorExtensions
{
    public static Vector3 Unit(this Vector3 vector) => Vector3.Divide(vector, vector.Length());

    public static bool NearZero(this Vector3 vector) => vector.X < float.Epsilon && vector.Y < float.Epsilon && vector.Z < float.Epsilon;

    public static float GetAxisByIndex(this Vector3 vector, int index)
    {
        switch (index)
        {
            case 0: return vector.X;
            case 1: return vector.Y;
            case 2: return vector.Z;
            default: throw new IndexOutOfRangeException($"The value {index} is out of range and must be 0 for X, 1 for Y, or 2 for Z.");
        }
    }
}