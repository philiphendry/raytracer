using System.Diagnostics;
using System.Numerics;

namespace RayTracer;

public static class Program
{
    [STAThread]
    static async Task Main()
    {
        var camera = new Camera(1000, 16.0f / 9.0f);
        var bitmap = new Bitmap(camera.ImageWidth, camera.ImageHeight);

        var worldObjects = new List<IHittable>
        {
            new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f),
            new Sphere(new Vector3(0.0f, -200.5f, -1.0f), 200.0f)
        };

        var timer = new Stopwatch();
        timer.Start();

        using var progressBar = new ShellProgressBar.ProgressBar(220, "Generating render chunks");
        var progress = new Progress<RenderProgress>(progress =>
        {
            progressBar.Message = $"Rendered {progress.CompletedChunkCount} of {progress.TotalChunkCount}";
            progressBar.MaxTicks = progress.TotalChunkCount;
            progressBar.Tick(progress.CompletedChunkCount);
        });

        await new Renderer(camera, worldObjects).RenderAsync(bitmap, progress);

        timer.Stop();
        Console.WriteLine($"Rendering took {timer.ElapsedMilliseconds}ms");

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