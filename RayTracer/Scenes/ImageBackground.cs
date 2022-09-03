using System.Numerics;
using RayTracer.Materials;
using RayTracer.Objects;

namespace RayTracer.Scenes;

public class ImageBackground : ISceneGenerator
{
    public void ApplySceneSettings(CommandLineOptions options)
    {
        options.Samples = 500;
        options.MaxDepth = 5;
        options.Width = 800;
        options.AspectRatio = "3:2";
        options.CameraPosition = "0,0,1.5";
        options.CameraLookAt = "0,0,-1";
        options.CameraVertical = "0,1,0";
        options.Aperture = 0.01f;
        options.FocusDistance = 10.0f;
        options.VerticalFieldOfView = 60.0f;
    }

    public World Build()
    {
        var materialGround = new LambertianMaterial(new Vector3(0.8f, 0.8f, 0.0f));
        var materialCenter = new LambertianMaterial(new Vector3(0.1f, 0.2f, 0.5f));
        var materialLeft = new DielectricMaterial(1.5f);
        var materialRight = new MetalMaterial(new Vector3(0.8f, 0.6f, 0.2f), 0.0f);

        IEnumerable<IHittable> worldObjects = new List<IHittable>
        {
            new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f, materialCenter),
            new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), 0.5f, materialLeft),
            new Sphere(new Vector3(1.0f, 0.0f, -1.0f), 0.5f, materialRight),
            new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100.0f, materialGround)
        };

        //return new World(worldObjects, new Objects.Backgrounds.ImageBackground(@".\Scenes\Images\solitude_interior.jpg"));
        //return new World(worldObjects, new Objects.Backgrounds.ImageBackground(@".\Scenes\Images\outdoor_field.jpg"));
        return new World(worldObjects, new Objects.Backgrounds.ImageBackground(@".\Scenes\Images\outdoor_shanghai.jpg"));
    }
}