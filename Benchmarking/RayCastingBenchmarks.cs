using System.Numerics;
using BenchmarkDotNet.Attributes;
using RayTracer;
using RayTracer.Objects;
using RayTracer.Scenes;

namespace Benchmarking;

[MemoryDiagnoser]
public class RayCastingBenchmarks
{
    private World _scene;
    private RayColourer _rayColourer;

    [GlobalSetup]
    public void Setup()
    {
        _scene = (new ManyBalls()).Build();
        _rayColourer = new RayColourer(false, false, false);
    }

    [Benchmark]
    public void RayCast()
    {
        _rayColourer.RayColour(new Ray(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, -1f)), _scene, 50);
    }
}