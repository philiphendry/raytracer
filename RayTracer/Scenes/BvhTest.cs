using System.Numerics;

namespace RayTracer.Scenes;

public class BvhTest : ISceneGenerator
{
    public IEnumerable<IHittable> Build(CommandLineOptions options)
    {
        if (options.UseSceneSettings)
        {
            options.Width = 1000;
            options.Height = 1000;
            options.Samples = 1;
            options.MaxDepth = 1;
            options.CameraPosition = "0,0,0";
            options.CameraLookAt = "0,0,-1";
            options.CameraVertical = "0,1,0";
            options.Aperture = 0.01f;
            options.FocusDistance = 1.0f;
            options.VerticalFieldOfView = 60.0f;
            options.NormalMaterial = true;
            options.DisableBvh = false;
            options.EnabledHitCounts = true;
        }

        var ballCount = 6000;
        var ballDiameter = 1f / ballCount;
        var ballRadius = ballDiameter / 2;
        var startX = ballDiameter * ballCount / -2f + ballRadius;
        var worldObjects = new List<IHittable>();

        for (var i = 0; i < ballCount; i++)
        {
            var sphere = new Sphere(new Vector3(startX, 0.0f, -1.0f), ballRadius, null!, enableHitCounts: options.EnabledHitCounts);
            startX += ballDiameter;
            worldObjects.Add(sphere);
        }

        return worldObjects;
    }
}