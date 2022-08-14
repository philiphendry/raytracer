using System.Numerics;

namespace RayTracer;

public class Camera
{
    private float AspectRatio { get; }
    public int ImageWidth { get; init; }
    public int ImageHeight { get; init; }

    private float ViewportHeight { get; }
    private float ViewportWidth { get; }

    private Vector3 Origin { get; }
    private Vector3 Horizontal { get; }
    private Vector3 Vertical { get; }
    private Vector3 LowerLeftCorner { get; }

    public Camera(int imageWidth, CommandLineOptions options)
    {
        var aspectRatio = options.GetAspectRatio();
        AspectRatio = aspectRatio.Item1 / aspectRatio.Item2;

        var theta = Utility.DegreesToRadians(options.VerticalFieldOfView);
        var h = (float)Math.Tan(theta / 2);
        ViewportHeight = 2.0f * h;
        ViewportWidth = AspectRatio * ViewportHeight;

        ImageWidth = imageWidth;
        ImageHeight = Convert.ToInt32(imageWidth / AspectRatio);

        Origin = Vector3Utility.FromString(options.CameraPosition);
        var lookAt = Vector3Utility.FromString(options.CameraLookAt);
        var vertical = Vector3Utility.FromString(options.CameraVertical);

        var w = (Origin - lookAt).Unit();
        var u = Vector3.Cross(vertical, w).Unit();
        var v = Vector3.Cross(w, u);

        Horizontal = Vector3.Multiply(u, ViewportWidth);
        Vertical = Vector3.Multiply(v, ViewportHeight);
        LowerLeftCorner = Origin - Vector3.Divide(Horizontal, 2f) - Vector3.Divide(Vertical, 2f) - w;
    }

    public Ray GetRay(float u, float v)
        => new(Origin, LowerLeftCorner + Vector3.Multiply(Horizontal, u) + Vector3.Multiply(Vertical, v) - Origin);
}