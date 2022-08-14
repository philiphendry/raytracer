﻿using System.Text.RegularExpressions;
using CommandLine;

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

    [Option("height", Required = false, HelpText = "The height of the rendered image in pixels.", Default = 1080)]
    public int Height { get; set; }

    [Option("aspectratio", Required = false, HelpText = "The aspect ratio of the rendered image expressed in the format n:m. Only provide width or height as the dimension not provided will be calculated.")]
    public string AspectRatio { get; set; }

    public (float, float) GetAspectRatio()
    {
        if (string.IsNullOrEmpty(AspectRatio))
            return (1.0f, (float)Height / Width);

        var match = Regex.Match(AspectRatio, @"(?<left>\d{1,4}):(?<right>\d{1,4})");
        return (float.Parse(match.Groups["left"].Value), float.Parse(match.Groups["right"].Value));
    }

    [Option("samples", Required = false, HelpText = "The number of samples taken for each pixel to anti-alias the final output.", Default = 20)]
    public int Samples { get; set; }

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
}