using System.Numerics;

namespace RayTracer.Objects.Backgrounds;

public interface IBackground
{
    Vector3 GetColour(Ray ray);
}