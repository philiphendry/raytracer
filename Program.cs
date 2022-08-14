using System.Diagnostics;
using System.Drawing.Imaging;
using System.Numerics;
using CommandLine;
using CommandLine.Text;
using RayTracer.Materials;

namespace RayTracer;

public static class Program
{
    [STAThread]
    static async Task Main(string[] args)
    {
        var parserResults = Parser.Default.ParseArguments<CommandLineOptions>(args);

        if (parserResults.Tag == ParserResultType.NotParsed)
        {
            await parserResults.WithNotParsedAsync(_ => Task.FromResult(1));
            return;
        }

        var message = OptionsValidator.Validate(parserResults.Value);
        if (!string.IsNullOrEmpty(message)){
            Console.WriteLine(HelpText.AutoBuild(parserResults, null, null));
            Console.WriteLine(message);
            Console.WriteLine();
            return;
        }

        await parserResults.WithParsedAsync(RunAsync);
    }

    private static async Task RunAsync(CommandLineOptions options)
    {
        var aspectRatio = options.GetAspectRatio();
        var camera = new Camera(options.Width, aspectRatio.Item1 / aspectRatio.Item2);
        var bitmap = new Bitmap(camera.ImageWidth, camera.ImageHeight);

        var materialGround = new LambertianMaterial(new Vector3(0.8f, 0.8f, 0.0f));
        var materialCenter = new LambertianMaterial(new Vector3(0.7f, 0.3f, 0.3f));
        var materialLeft = new MetalMaterial(new Vector3(0.8f, 0.8f, 0.8f));
        var materialRight = new MetalMaterial(new Vector3(0.8f, 0.6f, 0.2f));

        var worldObjects = new List<IHittable>
        {
            new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f, materialCenter),
            new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), 0.5f, materialLeft),
            new Sphere(new Vector3(1.0f, 0.0f, -1.0f), 0.5f, materialRight),
            new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100.0f, materialGround)
        };

        var timer = new Stopwatch();
        timer.Start();

        using var progressBar = new ShellProgressBar.ProgressBar(220, "Generating render chunks");
        var progress = new Progress<RenderProgress>(progress =>
        {
            // ReSharper disable AccessToDisposedClosure
            progressBar.Message = $"Rendered {progress.CompletedChunkCount} of {progress.TotalChunkCount}";
            progressBar.MaxTicks = progress.TotalChunkCount;
            progressBar.Tick(progress.CompletedChunkCount);
            // ReSharper restore AccessToDisposedClosure
        });

        await new Renderer(camera, worldObjects, options).RenderAsync(bitmap, progress);

        timer.Stop();
        Console.WriteLine($"Rendering took {timer.ElapsedMilliseconds}ms");

        var filename = $"render_{DateTime.Now.ToString("s").Replace(':', '-')}.png";
        if (options.SaveOutput)
        {
            bitmap.Save(filename, ImageFormat.Png);

            if (options.ViewOutput) 
                Process.Start(new ProcessStartInfo(filename) { UseShellExecute = true, Verb = "open" });
        }

        if (options.ViewInWindow)
            ShowBitmap(bitmap);
    }

    private static void ShowBitmap(Bitmap bitmap)
    {
        var pictureBox = new PictureBox();
        pictureBox.Size = new Size(bitmap.Width, bitmap.Height);
        pictureBox.Image = bitmap;
        var form = new Form();
        form.Controls.Add(pictureBox);
        form.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        form.AutoSize = true;
        Application.Run(form);
    }
}