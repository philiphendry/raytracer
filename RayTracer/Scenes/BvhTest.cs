using System.Numerics;
using RayTracer.Objects;

namespace RayTracer.Scenes;

public class BvhTest : ISceneGenerator
{
    public void ApplySceneSettings(CommandLineOptions options)
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
    }

    public World Build()
    {
        var ballCount = 60;
        var ballDiameter = 1f / ballCount;
        var ballRadius = ballDiameter / 2;
        var startX = ballDiameter * ballCount / -2f + ballRadius;
        var worldObjects = new List<IHittable>();

        for (var i = 0; i < ballCount; i++)
        {
            var sphere = new Sphere(new Vector3(startX, 0.0f, -1.0f), ballRadius, null!);
            startX += ballDiameter;
            worldObjects.Add(sphere);
        }

        return new World(worldObjects, new GraduatedBackground());
    }
}