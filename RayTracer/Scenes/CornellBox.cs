using System.Numerics;
using RayTracer.Materials;
using RayTracer.Objects;
using RayTracer.Objects.Backgrounds;
using RayTracer.Transforms;

namespace RayTracer.Scenes;

public sealed class CornellBox : ISceneGenerator
{
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

    public World Build()
    {
        var worldObjects = new List<IHittable>();

        var metalMaterial = new MetalMaterial(new Vector3(0.12f, 0.45f, 0.15f), 1.0f);
        var redMaterial = new LambertianMaterial(new Vector3(0.65f, 0.05f, 0.05f));
        var whiteMaterial = new LambertianMaterial(new Vector3(0.73f, 0.73f, 0.73f));
        var greenMaterial = new LambertianMaterial(new Vector3(0.12f, 0.45f, 0.15f));
        var glassMaterial = new DielectricMaterial(1.5f);
        var lightMaterial = new DiffuseLightMaterial(new Vector3(45.0f, 45.0f, 45.0f));

        worldObjects.Add(new YzRectangle(0.0f, 555.0f, 0.0f, 555.0f, 555.0f, greenMaterial)); // left wall
        worldObjects.Add(new YzRectangle(0.0f, 555.0f, 0.0f, 555.0f, 0.0f, redMaterial)); // right wall
        worldObjects.Add(new XzRectangle(213.0f, 343.0f, 227.0f, 332.0f, 554.0f, lightMaterial)); // ceiling light
        worldObjects.Add(new XzRectangle(0.0f, 555.0f, 0.0f, 555.0f, 555.0f, whiteMaterial)); // ceiling
        worldObjects.Add(new XzRectangle(0.0f, 555.0f, 0.0f, 555.0f, 0.0f, whiteMaterial)); // floor
        worldObjects.Add(new XyRectangle(0.0f, 555.0f, 0.0f, 555.0f, 555.0f, whiteMaterial)); // back wall

        //var sphere = new Sphere(new Vector3(100.0f, 470.0f, 100.0f), 50.0f, metalMaterial);
        //worldObjects.Add(new ConstantMedium(sphere, 0.05f, new Vector3(1.0f, 1.0f, 1.0f)));

        IHittable box1 = new Box(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(165.0f, 330.0f, 165.0f), whiteMaterial);
        box1 = new RotateY(box1, 15.0f);
        box1 = new Translate(box1, new Vector3(265.0f, 0.0f, 295.0f));
        worldObjects.Add(new ConstantMedium(box1, 0.01f, new Vector3(0.0f, 0.0f, 0.0f)));

        IHittable box2 = new Box(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(165.0f, 165.0f, 165.0f), glassMaterial);
        box2 = new RotateY(box2, -18.0f);
        box2 = new Translate(box2, new Vector3(130.0f, 0.0f, 65.0f));
        worldObjects.Add(new ConstantMedium(box2, 0.01f, new Vector3(1.0f, 1.0f, 1.0f)));

        var solidBlackBackground = new SolidBackground(new Vector3(0.0f, 0.0f, 0.0f));
        return new World(worldObjects, solidBlackBackground);
    }
}