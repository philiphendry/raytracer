using System.Numerics;
using RayTracer.Materials;
using RayTracer.Objects;

namespace RayTracer.SceneBuilder;

public class BoxBuilder : IBoxBuilder
{
    private float _sizeX = 1.0f;
    private float _sizeY = 1.0f;
    private float _sizeZ = 1.0f;
    private float _x;
    private float _y;
    private float _z;
    private IMaterial _material = WorldBuilder.DefaultWhiteMaterial;

    public IHittable Build() => new Box(new Vector3(_x, _y, _z), new Vector3(_x + _sizeX, _y + _sizeY, _z + _sizeZ), _material);

    public IBoxBuilder Size(float size)
    {
        _sizeX = size;
        _sizeY = size;
        _sizeZ = size;
        return this;
    }

    public IBoxBuilder Size(float sizeX, float sizeY, float sizeZ)
    {
        _sizeX = sizeX;
        _sizeY = sizeY;
        _sizeZ = sizeZ;
        return this;
    }

    public IBoxBuilder At(float x, float y, float z)
    {
        _x = x;
        _y = y;
        _z = z;
        return this;
    }

    public IBoxBuilder Material(IMaterial material)
    {
        _material = material;
        return this;
    }
}