using System.Text.RegularExpressions;

namespace RayTracer;

public static class OptionsValidator
{
    public static string? Validate(CommandLineOptions options)
    {
        if (options.Width != 0 && options.Height != 0 && !string.IsNullOrEmpty(options.AspectRatio))
        {
            return "When specifying aspectratio only provide either width or height.";
        }

        if (options.ViewOutput && !options.SaveOutput)
        {
            return "When specifying viewoutput you must also provide saveoutput.";
        }

        if (!string.IsNullOrEmpty(options.AspectRatio) && !Regex.IsMatch(options.AspectRatio, @"^\d{1,4}:\d{1,4}$"))
        {
            return "The aspectratio must be provided as two integers up to four digits long and separated by a colon.";
        }

        if (string.IsNullOrEmpty(options.AspectRatio) && (options.Width == 0 || options.Height == 0))
        {
            return "Provide both width and height or the aspectratio.";
        }

        if (options.DisableLambertian && !options.DisableMaterials)
        {
            return "Disabling Lambertian has no effect whilst materials are enabled";
        }

        var vectorRegex = new Regex(@"^(?<x>-?\d+(\.\d+)?),(?<y>-?\d+(\.\d+)?),(?<z>-?\d+(\.\d+)?)$");

        if (!vectorRegex.IsMatch(options.CameraPosition))
        {
            return "The camera-position must be specified as three numbers separated by commas e.g. 1.0,2.0,-2.0.";
        }

        if (!vectorRegex.IsMatch(options.CameraLookAt))
        {
            return "The camera-look-at must be specified as three numbers separated by commas e.g. 1.0,2.0,-2.0.";
        }

        if (!vectorRegex.IsMatch(options.CameraVertical))
        {
            return "The camera-vertical must be specified as three numbers separated by commas e.g. 1.0,2.0,-2.0.";
        }

        return null;
    }
}