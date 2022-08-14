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

        return null;
    }
}