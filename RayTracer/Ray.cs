using System.Numerics;

namespace RayTracer;

public sealed class Ray
{
    public Ray(Vector3 origin, Vector3 direction)
    {
        Origin = origin;
        Direction = direction;
    }

    public Vector3 Origin { get; init; }
    public Vector3 Direction { get; init; }
    public Vector3 PositionAt(float t) => Origin + Vector3.Multiply(Direction, t);
    public override string ToString() => $"Origin X={Origin.X},Y={Origin.Y},Z={Origin.Z}, Direction X={Direction.X},Y={Direction.Y},Z={Direction.Z}";
}