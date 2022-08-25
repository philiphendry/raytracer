using RayTracer.Objects;

namespace RayTracer.Scenes;

public interface ISceneGenerator
{
    World Build(CommandLineOptions options);
}