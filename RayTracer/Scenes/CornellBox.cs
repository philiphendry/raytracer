using System.Collections.Immutable;
using System.Numerics;
using RayTracer.Materials;
using RayTracer.Objects;
using RayTracer.Textures;

namespace RayTracer.Scenes;

public class CornellBox : ISceneGenerator
{
    public World Build(CommandLineOptions options)
    {
        if (options.UseSceneSettings)
        {
            options.Width = 600;
            options.AspectRatio = "1:1";
            options.Samples = 600;
            options.MaxDepth = 5;
            options.CameraPosition = "278,278,-800";
            options.CameraLookAt = "278,278,0";
            options.CameraVertical = "0,1,0";
            options.Aperture = 32.1f;
            options.FocusDistance = 800.0f;
            options.VerticalFieldOfView = 40.0f;
            options.ChunkSize = 50;
            options.DisableBvh = false;
            options.EnabledHitCounts = false;
        }

        var worldObjects = new List<IHittable>();

        var redMaterial = new LambertianMaterial(new Vector3(0.65f, 0.05f, 0.05f));
        var whiteMaterial = new LambertianMaterial(new Vector3(0.73f, 0.73f, 0.73f));
        var greenMaterial = new LambertianMaterial(new Vector3(0.12f, 0.45f, 0.15f));
        var lightMaterial = new DiffuseLightMaterial(new Vector3(15.0f, 15.0f, 15.0f));
        
        worldObjects.Add(new YzRectangle(0.0f, 555.0f, 0.0f, 555.0f, 555.0f, greenMaterial, options));
        worldObjects.Add(new YzRectangle(0.0f, 555.0f, 0.0f, 555.0f, 0.0f, redMaterial, options));
        worldObjects.Add(new XzRectangle(213.0f, 343.0f, 227.0f, 332.0f, 554.0f, lightMaterial, options));
        worldObjects.Add(new XzRectangle(0.0f, 555.0f, 0.0f, 555.0f, 0.0f, whiteMaterial, options));
        worldObjects.Add(new XzRectangle(0.0f, 555.0f, 0.0f, 555.0f, 555.0f, whiteMaterial, options));
        worldObjects.Add(new XyRectangle(0.0f, 555.0f, 0.0f, 555.0f, 555.0f, whiteMaterial, options));

        worldObjects.Add(new Box(new Vector3(130.0f, 0.0f, 65.0f), new Vector3(295.0f, 165.0f, 230.0f), whiteMaterial, options));
        worldObjects.Add(new Box(new Vector3(265.0f, 0.0f, 295.0f), new Vector3(430, 330.0f, 460.0f), whiteMaterial, options));

        var solidBlackBackground = new SolidBackground(new Vector3(0.0f, 0.0f, 0.0f));
        return new World(worldObjects.ToImmutableArray(), solidBlackBackground, options);
    }
}