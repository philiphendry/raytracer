using System.Numerics;
using RayTracer.Materials;
using RayTracer.Objects;
using RayTracer.Objects.Backgrounds;
using RayTracer.Textures;

namespace RayTracer.SceneBuilder;

public class WorldBuilder : IWorldBuilder
{
    public static readonly IMaterial DefaultWhiteMaterial = new LambertianMaterial(new SolidColourTexture(new Vector3(1.0f, 1.0f, 1.0f)));

    private readonly List<IHittable> _worldObjects = new();
    private IBackground? _background;

    private WorldBuilder() { }

    public static IWorldBuilder Create() => new WorldBuilder();


    public IWorldBuilder AddBackground(IBackground background)
    {
        if (_background != null)
            throw new InvalidOperationException($"Only one call to {nameof(AddBackground)} can be made.");

        _background = background;
        return this;
    }

    public World Build()
    {
        if (_background == null)
            throw new InvalidOperationException($"A background must be added with a call to {nameof(AddBackground)}().");

        return new World(_worldObjects, _background);
    }

    public IWorldBuilder AddObject(IObjectBuilder objectBuilder)
    {
        _worldObjects.Add(objectBuilder.Build());
        return this;
    }
}