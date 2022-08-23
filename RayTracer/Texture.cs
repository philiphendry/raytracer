using System.Numerics;

namespace RayTracer;

public abstract class Texture
{
    public abstract Vector3 Value(float u, float v, Vector3 p);
}

public class SolidColour : Texture
{
    private readonly Vector3 _colour;

    public SolidColour(Vector3 colour) => _colour = colour;

    public override Vector3 Value(float u, float v, Vector3 p) => _colour;
}