using System.Collections.Immutable;
using System.Numerics;
using RayTracer.Objects;

namespace RayTracer.Scenes;

public class BvhTest : ISceneGenerator
{
    public World Build(CommandLineOptions options)
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

        var ballCount = 60;
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

        return options.DisableBvh
            ? new World(worldObjects.ToImmutableArray(), new GraduatedBackground(), options)
            : new World(new[] { new BoundedVolumeHierarchyNode(worldObjects.ToImmutableArray(), options) }.ToImmutableArray<IHittable>(), new GraduatedBackground(), options);
    }
}