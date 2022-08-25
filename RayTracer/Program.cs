using System.Collections.Immutable;
using System.Diagnostics;
using System.Drawing.Imaging;
using CommandLine;
using CommandLine.Text;
using RayTracer.Utilities;
using ProgressBar = ShellProgressBar.ProgressBar;

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
        var world = Utility.GetSceneGenerator(options.RenderSceneName)!.Build(options);
        var camera = new Camera(options.Width, options);
        var bitmap = new Bitmap(camera.ImageWidth, camera.ImageHeight);

        var timer = new Stopwatch();
        timer.Start();

        using var progressBar = new ProgressBar(220, "Generating render chunks");
        var progress = new Progress<RenderProgress>(progress =>
        {
            // ReSharper disable AccessToDisposedClosure
            progressBar.Message = $"Rendered {progress.Completed} of {progress.Total}";
            progressBar.MaxTicks = progress.Total;
            progressBar.Tick(progress.Completed);
            // ReSharper restore AccessToDisposedClosure
        });

        using var cancellationTokenSource = new CancellationTokenSource();
        Console.CancelKeyPress += (_, _) =>
        {
            Console.WriteLine("Exiting...");
            cancellationTokenSource.Cancel();
        };

        await new Renderer(camera, world, options)
            .RenderAsync(bitmap, progress, cancellationTokenSource.Token);

        if (cancellationTokenSource.IsCancellationRequested)
            return;

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