using RayTracer.Materials;

namespace RayTracer.SceneBuilder;

public interface ISphereBuilder : IObjectBuilder
{
    ISphereBuilder Material(IMaterial material);
    ISphereBuilder At(float x, float y, float z);
    ISphereBuilder Radius(float radius);
}