using System.Numerics;
using RayTracer.Materials;
using RayTracer.Objects;
using RayTracer.Objects.Backgrounds;
using RayTracer.Textures;

namespace RayTracer.Scenes;

public sealed class EmissiveMaterials : ISceneGenerator
{
    public void ApplySceneSettings(CommandLineOptions options)
    {
        options.Width = 800;
        options.AspectRatio = "3:2";
        options.Samples = 400;
        options.MaxDepth = 5;
        options.CameraPosition = "26,3,6";
        options.CameraLookAt = "0,2,0";
        options.CameraVertical = "0,1,0";
        options.Aperture = 0.1f;
        options.FocusDistance = 26.0f;
        options.VerticalFieldOfView = 20.0f;
        options.ChunkSize = 20;
    }

    public World Build()
    {
        var worldObjects = new List<IHittable>();

        var material3 = new LambertianMaterial(new ImageTexture(@"Textures\Images\earthmap.jpg"));
        worldObjects.Add(new Sphere(new Vector3(0.0f, 2.0f, 0.0f), 2.0f, material3));

        var groundMaterial = new LambertianMaterial(new CheckerTexture(new Vector3(0.2f, 0.3f, 0.1f), new Vector3(0.9f, 0.9f, 0.9f)));
        worldObjects.Add(new Sphere(new Vector3(0.0f, -1000.0f, 0.0f), 1000.0f, groundMaterial));

        var diffuseLight = new DiffuseLightMaterial(new Vector3(8.0f, 8.0f, 8.0f));
        worldObjects.Add(new XyRectangle(3.0f, 5.0f, 1.0f, 3.0f, -2.0f, diffuseLight));

        return new World(worldObjects, new SolidBackground(new Vector3(0, 0, 0)));
    }
}