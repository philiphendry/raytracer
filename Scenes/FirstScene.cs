using System.Numerics;
using RayTracer.Materials;

namespace RayTracer.Scenes;

public class FirstScene : ISceneGenerator
{
    public IEnumerable<IHittable> Build(CommandLineOptions options)
    {
        options.CameraPosition = "0,0,0";
        options.CameraLookAt = "0,0,-1";
        options.CameraVertical = "0,1,0";
        options.Aperture = 0.01f;
        options.FocusDistance = 10.0f;
        options.VerticalFieldOfView = 60.0f;

        var materialGround = new LambertianMaterial(new Vector3(0.8f, 0.8f, 0.0f));
        var materialCenter = new LambertianMaterial(new Vector3(0.1f, 0.2f, 0.5f));
        var materialLeft = new DielectricMaterial(1.5f);
        var materialRight = new MetalMaterial(new Vector3(0.8f, 0.6f, 0.2f), 0.0f);

        IEnumerable<IHittable> worldObjects = new List<IHittable>
        {
            new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f, materialCenter, enableHitCounts: options.EnabledHitCounts),
            new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), 0.5f, materialLeft, enableHitCounts: options.EnabledHitCounts),
            new Sphere(new Vector3(1.0f, 0.0f, -1.0f), 0.5f, materialRight, enableHitCounts: options.EnabledHitCounts),
            new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100.0f, materialGround, enableHitCounts: options.EnabledHitCounts)
        };

        return worldObjects;
    }
}