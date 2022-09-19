using System.Numerics;
using RayTracer.Materials;
using RayTracer.Objects;

namespace RayTracer.SceneBuilder;

public class SphereBuilder : ISphereBuilder
{
    private float _radius = 1.0f;
    private float _x;
    private float _y;
    private float _z;
    private IMaterial _material = WorldBuilder.DefaultWhiteMaterial;

    public IHittable Build() => new Sphere(new Vector3(_x, _y, _z), _radius, _material);

    public ISphereBuilder Radius(float radius)
    {
        _radius = radius;
        return this;
    }

    public ISphereBuilder At(float x, float y, float z)
    {
        _x = x;
        _y = y;
        _z = z;
        return this;
    }

    public ISphereBuilder Material(IMaterial material)
    {
        _material = material;
        return this;
    }
}