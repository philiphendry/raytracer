using System.Numerics;

namespace RayTracer.Objects;

public class SolidBackground : IBackground
{
    private readonly Vector3 _colour;

    public SolidBackground(Vector3 colour) => _colour = colour;

    public Vector3 GetColour(Ray ray) => _colour;
}