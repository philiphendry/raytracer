using System.Numerics;

namespace RayTracer.Objects;

public interface IBackground
{
    Vector3 GetColour(Ray ray);
}