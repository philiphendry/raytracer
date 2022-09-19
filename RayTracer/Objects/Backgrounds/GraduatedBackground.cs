using System.Numerics;
using RayTracer.Utility;

namespace RayTracer.Objects.Backgrounds;

public sealed class GraduatedBackground : IBackground
{
    private readonly Vector3 _white;
    private readonly Vector3 _blue;

    public GraduatedBackground()
    {
        _white = new Vector3(1.0f, 1.0f, 1.0f);
        _blue = new Vector3(0.5f, 0.7f, 1.0f);
    }

    public Vector3 GetColour(Ray ray)
    {
        var t = 0.5f * (ray.Direction.Unit().Y + 1.0f);
        return Vector3.Add(Vector3.Multiply(_white, 1.0f - t), Vector3.Multiply(_blue, t));
    }
}