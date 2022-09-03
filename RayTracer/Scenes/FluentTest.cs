using RayTracer.Materials;
using RayTracer.Objects;
using RayTracer.Textures;
using System.Numerics;
using RayTracer.Objects.Backgrounds;

namespace RayTracer.Scenes;

public class FluentTest : ISceneGenerator
{
    public World Build()
    {
        var metalMaterial = new MetalMaterial(new Vector3(0.9f, 0.1f, 0.1f), 0.5f);

        return WorldBuilder
            .Create()
            .AddBackground(new GraduatedBackground())
            .AddSphere().At(10, 10, 10).Radius(10).Build()
            .AddSphere().At(30, 30, 30).Radius(15).Material(metalMaterial).Build()
            .AddBox().At(40, 40, 40).Size(10).Material(metalMaterial).Build()
            .Build();
    }

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
}

public class WorldBuilder
{
    public static readonly IMaterial DefaultWhiteMaterial = new LambertianMaterial(new SolidColourTexture(new Vector3(1.0f, 1.0f, 1.0f)));

    private readonly List<IHittable> _worldObjects = new();
    private IBackground? _background;

    private WorldBuilder() { }

    public static WorldBuilder Create() => new();


    public WorldBuilder AddBackground(IBackground background)
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

    public SphereBuilder<WorldBuilder> AddSphere() =>
        new(sphere =>
        {
            _worldObjects.Add(sphere);
            return this;
        });

    public BoxBuilder<WorldBuilder> AddBox() =>
        new(box =>
        {
            _worldObjects.Add(box);
            return this;
        });
}

public abstract class ObjectBuilderBase<TObj, TReturn>
{
    // ReSharper disable InconsistentNaming
    private protected readonly Func<TObj, TReturn> _returnCallback;
    private protected IMaterial _material = WorldBuilder.DefaultWhiteMaterial;
    private protected float _x;
    private protected float _y;
    private protected float _z;
    // ReSharper restore InconsistentNaming

    protected ObjectBuilderBase(Func<TObj, TReturn> returnCallback) => _returnCallback = returnCallback;

    protected abstract TObj Construct();

    public TReturn Build() => _returnCallback(Construct());

    public virtual ObjectBuilderBase<TObj, TReturn> Material(IMaterial material)
    {
        _material = material;
        return this;
    }

    public virtual ObjectBuilderBase<TObj, TReturn> At(float x, float y, float z)
    {
        _x = x;
        _y = y;
        _z = z;
        return this;
    }
}

public class BoxBuilder<T> : ObjectBuilderBase<Box, T>
{
    private float _maxX = 1.0f;
    private float _maxY = 1.0f;
    private float _maxZ = 1.0f;

    public BoxBuilder(Func<Box, T> returnCallback) : base(returnCallback)
    {
    }

    protected override Box Construct()
    {
        return new(new Vector3(_x, _y, _z), new Vector3(_maxX, _maxY, _maxZ), _material);
    }

    public override BoxBuilder<T> At(float x, float y, float z) => (BoxBuilder<T>)base.At(x, y, z);

    public override BoxBuilder<T> Material(IMaterial material) => (BoxBuilder<T>)base.Material(material);

    public BoxBuilder<T> Size(float size) => Size(size, size, size);

    public BoxBuilder<T> Size(float sizeX, float sizeY, float sizeZ)
    {
        _maxX = sizeX + _x;
        _maxY = sizeY + _y;
        _maxZ = sizeZ + _z;
        return this;
    }
}

public class SphereBuilder<T> : ObjectBuilderBase<Sphere, T>
{
    private float _radius = 1.0f;

    public SphereBuilder(Func<Sphere, T> returnCallback) : base(returnCallback)
    {
    }

    protected override Sphere Construct() => new(new Vector3(_x, _y, _z), _radius, _material);

    public override SphereBuilder<T> At(float x, float y, float z) => (SphereBuilder<T>)base.At(x, y, z);

    public override SphereBuilder<T> Material(IMaterial material) => (SphereBuilder<T>)base.Material(material);

    public SphereBuilder<T> Radius(float radius)
    {
        _radius = radius;
        return this;
    }
}