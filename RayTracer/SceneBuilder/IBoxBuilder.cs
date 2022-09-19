using RayTracer.Materials;

namespace RayTracer.SceneBuilder;

public interface IBoxBuilder : IObjectBuilder
{
    IBoxBuilder Material(IMaterial material);
    IBoxBuilder At(float x, float y, float z);
    IBoxBuilder Size(float size);
    IBoxBuilder Size(float sizeX, float sizeY, float sizeZ);
}