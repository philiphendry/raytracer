using System.Numerics;
using RayTracer.Materials;
using RayTracer.Objects;
using RayTracer.Objects.Backgrounds;
using RayTracer.Textures;
using RayTracer.Utility;

namespace RayTracer.Scenes;

public class ManyBalls : ISceneGenerator
{
    public void ApplySceneSettings(CommandLineOptions options)
    {
        options.Width = 800;
        options.AspectRatio = "3:2";
        options.Samples = 40;
        options.MaxDepth = 5;
        options.CameraPosition = "13,2,3";
        options.CameraLookAt = "0,0,0";
        options.CameraVertical = "0,1,0";
        options.Aperture = 0.1f;
        options.FocusDistance = 10.0f;
        options.VerticalFieldOfView = 20.0f;
        options.ChunkSize = 20;
    }

    public World Build()
    {
        var worldObjects = new List<IHittable>();

        var distributionSize = 11;
        var ballsPerCell = 1;

        for (var a = -distributionSize; a < distributionSize; a++)
        {
            for (var b = -distributionSize; b < distributionSize; b++)
            {
                for (int i = 0; i < ballsPerCell; i++)
                {
                    var origin = new Vector3(a + 0.9f * Util.Random(), 0.2f, b + 0.9f * Util.Random());
                    if ((origin - new Vector3(4.0f, 0.2f, 0.0f)).Length() > 0.9f)
                    {
                        var randomMaterial = Util.Random();
                        if (randomMaterial < 0.7)
                        {
                            var albedo = Vector3Utility.Random() * Vector3Utility.Random();
                            var material = new LambertianMaterial(albedo);
                            worldObjects.Add(new Sphere(origin, 0.2f, material));
                        } 
                        else if (randomMaterial < 0.9)
                        {
                            var albedo = Vector3Utility.Random(0.5f, 1.0f);
                            var fuzz = Util.Random(0.0f, 0.5f);
                            var material = new MetalMaterial(albedo, fuzz);
                            worldObjects.Add(new Sphere(origin, 0.2f, material));
                        }
                        else
                        {
                            var material = new DielectricMaterial(1.5f);
                            worldObjects.Add(new Sphere(origin, 0.2f, material));
                        }
                    }
                }
            }
        }

        var material1 = new DielectricMaterial(1.5f);
        worldObjects.Add(new Sphere(new Vector3(0.0f, 1.0f, 0.0f), 1.0f, material1));

        var material2 = new MetalMaterial(new Vector3(0.7f, 0.6f, 0.5f), 0.0f);
        worldObjects.Add(new Sphere(new Vector3(-4.0f, 1.0f, 0.0f), 1.0f, material2));

        var material3 = new LambertianMaterial(new ImageTexture(@"Textures\Images\earthmap.jpg"));
        worldObjects.Add(new Sphere(new Vector3(4.0f, 1.0f, 0.0f), 1.0f, material3));

        var groundMaterial = new LambertianMaterial(new CheckerTexture(new Vector3(0.2f, 0.3f, 0.1f), new Vector3(0.9f, 0.9f, 0.9f)));
        worldObjects.Add(new Sphere(new Vector3(0.0f, -1000.0f, 0.0f), 1000.0f, groundMaterial));

        return new World(worldObjects, new GraduatedBackground());
    }
}