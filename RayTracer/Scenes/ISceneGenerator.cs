using RayTracer.Objects;

namespace RayTracer.Scenes;

public interface ISceneGenerator
{
    World Build();

    void ApplySceneSettings(CommandLineOptions options);
}