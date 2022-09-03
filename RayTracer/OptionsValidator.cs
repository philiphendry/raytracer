using System.Text.RegularExpressions;
using RayTracer.Utility;

namespace RayTracer;

public static class OptionsValidator
{
    public static string? Validate(CommandLineOptions options)
    {
        if (options.Width != 0 && options.Height != 0 && !string.IsNullOrEmpty(options.AspectRatio))
        {
            return "When specifying aspectratio only provide either width or height.";
        }

        if (options.Height == 0 && string.IsNullOrEmpty(options.AspectRatio))
        {
            options.Height = 1080;
        }

        if (options.ViewOutput && !options.SaveOutput)
        {
            return "When specifying --view you must also provide --save.";
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

        if (!Vector3Utility.VectorRegex.IsMatch(options.CameraPosition!))
        {
            return "The camera-position must be specified as three numbers separated by commas e.g. 1.0,2.0,-2.0.";
        }

        if (!Vector3Utility.VectorRegex.IsMatch(options.CameraLookAt!))
        {
            return "The camera-look-at must be specified as three numbers separated by commas e.g. 1.0,2.0,-2.0.";
        }

        if (!Vector3Utility.VectorRegex.IsMatch(options.CameraVertical!))
        {
            return "The camera-vertical must be specified as three numbers separated by commas e.g. 1.0,2.0,-2.0.";
        }

        if (Util.GetSceneGenerator(options.RenderSceneName) == null)
        {
            return $"The render-built-in-scene of '{options.RenderSceneName}' cannot be found as a built-in scene.";
        }

        return null;
    }
}