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

    public static Color ToColour(this Vector3 vectorColour, int samplesPerPixel)
    {
        var scale = 1.0f / samplesPerPixel;

        // Gamma correct with a factor of 2 which means raising colour to the power 1/gamma which in this case is the square root
        var r = (float)Math.Sqrt(scale * vectorColour.X);
        var g = (float)Math.Sqrt(scale * vectorColour.Y);
        var b = (float)Math.Sqrt(scale * vectorColour.Z);

        return Color.FromArgb(
            (int)(Math.Clamp(r, 0.0f, 0.999f) * 256), 
            (int)(Math.Clamp(g, 0.0f, 0.999f) * 256),
            (int)(Math.Clamp(b, 0.0f, 0.999f) * 256));
    }
}