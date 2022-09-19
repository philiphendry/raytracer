using RayTracer.Objects;
using RayTracer.Objects.Backgrounds;

namespace RayTracer.SceneBuilder;

public interface IWorldBuilder
{
    World Build();
    IWorldBuilder AddBackground(IBackground background);
    IWorldBuilder AddObject(IObjectBuilder objectBuilder);
}