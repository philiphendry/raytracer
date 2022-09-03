using System.Numerics;
using RayTracer.Materials;
using RayTracer.Textures;

namespace RayTracer.Objects.Backgrounds;

public class ImageBackground : IBackground
{
    private readonly Sphere _sphere;

    public ImageBackground(string filename)
    {
        var imageTexture = new ImageTexture(filename);
        _sphere = new Sphere(Vector3.Zero, 1000.0f, new DiffuseLightMaterial(imageTexture));
    }

    public Vector3 GetColour(Ray ray)
    {
        var hitPoint = _sphere.Hit(ray, 0.0001f, float.PositiveInfinity);
        return hitPoint!.Value.Material.Emitted(hitPoint.Value.U, hitPoint.Value.V, hitPoint.Value.Point);
    }
}