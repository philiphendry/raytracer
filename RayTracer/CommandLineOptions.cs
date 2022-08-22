using System.Text.RegularExpressions;
using CommandLine;
using RayTracer.Scenes;

namespace RayTracer;

public class CommandLineOptions
{
    [Option("saveoutput", Required = false, HelpText = "Saves the rendered output to a file. The filename is automatically generated.")]
    public bool SaveOutput { get; set; }

    [Option("viewinwindow", Required = false, HelpText = "Displays the rendered output in a window.")]
    public bool ViewInWindow { get; set; }

    [Option("viewoutput", Required = false, HelpText = "Asks the operating system to display the rendered output with a default image viewer.")]
    public bool ViewOutput { get; set; }

    [Option("width", Required = false, HelpText = "The width of the rendered image in pixels.", Default = 1920)]
    public int Width { get; set; }

    [Option("height", Required = false, HelpText = "The height of the rendered image in pixels.")]
    public int Height { get; set; }

    [Option("render-built-in-scene", Required = false, HelpText = "Provide the name of a built-in scene.", Default = nameof(FirstScene))]
    public string? RenderSceneName { get; set; }

    [Option("use-scene-settings", Required = false, HelpText = "Allows the built-in scene to supply its own settings overridding those given on the command-line.")]
    public bool UseSceneSettings { get; set; }

    [Option("aspectratio", Required = false, HelpText = "The aspect ratio of the rendered image expressed in the format n:m. Only provide width or height as the dimension not provided will be calculated.")]
    public string? AspectRatio { get; set; }

    public (float, float) GetAspectRatio()
    {
        if (string.IsNullOrEmpty(AspectRatio))
            return (1.0f, (float)Height / Width);

        var match = Regex.Match(AspectRatio, @"(?<left>\d{1,4}):(?<right>\d{1,4})");
        return (float.Parse(match.Groups["left"].Value), float.Parse(match.Groups["right"].Value));
    }

    [Option("samples", Required = false, HelpText = "The number of samples taken for each pixel to anti-alias the final output.", Default = 20)]
    public int Samples { get; set; }

    [Option("parallelism", Required = false, HelpText = "The maximum degree of parallelism. The default of 0 causes all CPU logical cores to be used. Any value over the number of logical cores is ignored and the default used instead.", Default = 0)]
    public int Parallelism { get; set; }

    [Option("disablebvh", Required = false, HelpText = "Disables the Bounded Volume Hierarchy functionality that makes ray path tracing more efficient.")]
    public bool DisableBvh { get; set; }

    [Option("enable-hit-counts", Required = false, HelpText = "Enables counting of ray hits and outputs the results. For debugging purposes only has there is a performance impact.")]
    public bool EnabledHitCounts { get; set; }

    [Option("maxdepth", Required = false, HelpText = "The maximum depth to trace rays.", Default = 10)]
    public int MaxDepth { get; set; }

    [Option("chunksize", Required = false, HelpText = "The size to 'chunk' the image into and run in parallel.", Default = 50)]
    public int ChunkSize { get; set; }

    [Option("normal-material", Required = false, HelpText = "Replaces all materials with a representation of the normal.", Default = false)]
    public bool NormalMaterial { get; set; }

    [Option("disable-materials", Required = false, HelpText = "Turns materials off and just calculates shading.", Default = false)]
    public bool DisableMaterials { get; set; }

    [Option("disable-lambertian", Required = false, HelpText = "Disable Lambertian distribution for calculating diffuse materials and use alternative algorithm.", Default = false)]
    public bool DisableLambertian { get; set; }

    [Option("field-of-view", Required = false, HelpText = "The vertical field of view in degrees.", Default = 60.0f)]
    public float VerticalFieldOfView { get; set; }

    [Option("aperture", Required = false, HelpText = "The aperture of the camera with larger values producing greater depth-of-field.", Default = 0.5f)]
    public float Aperture { get; set; }

    [Option("focus-distance", Required = false, HelpText = "The distance from camera to viewport.", Default = 10.0f)]
    public float FocusDistance { get; set; }

    [Option("camera-position", Required = false, HelpText="A position for the camera", Default = "0.0,0.0,0.0")]
    public string? CameraPosition { get; set; }

    [Option("camera-look-at", Required = false, HelpText = "A position for the camera to look at", Default = "0.0,0.0,-1.0")]
    public string? CameraLookAt { get; set; }

    [Option("camera-vertical", Required = false, HelpText = "A vector for the camera vertical position", Default = "0.0,1.0,0.0")]
    public string? CameraVertical { get; set; }
}