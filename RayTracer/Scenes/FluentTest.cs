using RayTracer.Materials;
using RayTracer.Objects;
using System.Numerics;
using RayTracer.Objects.Backgrounds;
using RayTracer.SceneBuilder;

namespace RayTracer.Scenes;

public sealed class FluentTest : ISceneGenerator
{
    public World Build()
    {
        var metalMaterial = new MetalMaterial(new Vector3(0.9f, 0.1f, 0.1f), 0.5f);

        return WorldBuilder
            .Create()
            .AddBackground(new GraduatedBackground())
            .AddObject(new SphereBuilder().At(10, 10, 10).Radius(10))
            .AddObject(new SphereBuilder().At(30, 30, 30).Radius(15).Material(metalMaterial))
            .AddObject(new BoxBuilder().At(40, 40, 40).Size(10).Material(metalMaterial))
            .Build();
    }

    public void ApplySceneSettings(CommandLineOptions options)
    {
        options.Width = 600;
        options.AspectRatio = "1:1";
        options.Samples = 1000;
        options.MaxDepth = 5;
        options.CameraPosition = "278,278,-800";
        options.CameraLookAt = "278,278,0";
        options.CameraVertical = "0,1,0";
        options.Aperture = 0.0f;
        options.FocusDistance = 800.0f;
        options.VerticalFieldOfView = 40.0f;
        options.ChunkSize = 50;
    }
}