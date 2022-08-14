using System.Collections.Immutable;
using System.Numerics;
using RayTracer.Materials;

namespace RayTracer.Scenes;

public class ManyBalls : ISceneGenerator
{
    public IEnumerable<IHittable> Build(CommandLineOptions options)
    {
        options.Width = 800;
        options.AspectRatio = "3:2";
        options.Samples = 500;
        options.MaxDepth = 50;
        options.CameraPosition = "13,2,3";
        options.CameraLookAt = "0,0,0";
        options.CameraVertical = "0,1,0";
        options.Aperture = 0.1f;
        options.FocusDistance = 10.0f;
        options.VerticalFieldOfView = 20.0f;

        var worldObjects = new List<IHittable>();

        var groundMaterial = new LambertianMaterial(new Vector3(0.5f, 0.5f, 0.5f));

        for (var a = -11; a < 11; a++)
        {
            for (var b = -11; b < 11; b++)
            {
                var origin = new Vector3(a + 0.9f * Utility.Random(), 0.2f, b + 0.9f * Utility.Random());
                if ((origin - new Vector3(4.0f, 0.2f, 0.0f)).Length() > 0.9f)
                {
                    var randomMaterial = Utility.Random();
                    if (randomMaterial < 0.7)
                    {
                        var albedo = Vector3Utility.Random() * Vector3Utility.Random();
                        var material = new LambertianMaterial(albedo);
                        worldObjects.Add(new Sphere(origin, 0.2f, material));
                    } 
                    else if (randomMaterial < 0.9)
                    {
                        var albedo = Vector3Utility.Random(0.5f, 1.0f);
                        var fuzz = Utility.Random(0.0f, 0.5f);
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

        var material1 = new DielectricMaterial(1.5f);
        worldObjects.Add(new Sphere(new Vector3(0.0f, 1.0f, 0.0f), 1.0f, material1));

        var material2 = new LambertianMaterial(new Vector3(0.4f, 0.2f, 0.1f));
        worldObjects.Add(new Sphere(new Vector3(-4.0f, 1.0f, 0.0f), 1.0f, material2));

        var material3 = new MetalMaterial(new Vector3(0.7f, 0.6f, 0.5f), 0.0f);
        worldObjects.Add(new Sphere(new Vector3(4.0f, 1.0f, 0.0f), 1.0f, material3));

        worldObjects.Add(new Sphere(new Vector3(0.0f, -1000.0f, 0.0f), 1000.0f, groundMaterial));


        return worldObjects.ToImmutableArray();
    }
}