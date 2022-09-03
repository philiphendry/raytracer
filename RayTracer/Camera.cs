using System.Numerics;
using RayTracer.Utility;

namespace RayTracer;

public class Camera
{
    private readonly float _lensRadius;
    private readonly Vector3 _origin;
    private readonly Vector3 _horizontal;
    private readonly Vector3 _vertical;
    private readonly Vector3 _lowerLeftCorner;
    private readonly Vector3 _u;
    private readonly Vector3 _v;

    public int ImageWidth { get; }
    public int ImageHeight { get; }

    public Camera(CommandLineOptions options)
    {
        var aspectRatio = options.GetAspectRatio();

        var theta = Util.DegreesToRadians(options.VerticalFieldOfView);
        var h = MathF.Tan(theta / 2);
        var viewportHeight = 2.0f * h;
        var viewportWidth = aspectRatio * viewportHeight;

        ImageWidth = options.Width;
        ImageHeight = Convert.ToInt32(options.Width / aspectRatio);

        _origin = Vector3Utility.FromString(options.CameraPosition);
        var lookAt = Vector3Utility.FromString(options.CameraLookAt);
        var vertical = Vector3Utility.FromString(options.CameraVertical);

        var w = (_origin - lookAt).Unit();
        _u = Vector3.Cross(vertical, w).Unit();
        _v = Vector3.Cross(w, _u);

        var focusDistance = options.FocusDistance;

        _horizontal = Vector3.Multiply(_u, viewportWidth * focusDistance);
        _vertical = Vector3.Multiply(_v, viewportHeight * focusDistance);
        _lowerLeftCorner = _origin - Vector3.Divide(_horizontal, 2f) - Vector3.Divide(_vertical, 2f) - w * focusDistance;

        _lensRadius = options.Aperture / 2.0f;
    }

    public Ray GetRay(float u, float v)
    {
        var randomPoint = _lensRadius * Vector3Utility.RandomInUnitDisk();
        var offset = Vector3.Multiply(_u, randomPoint.X) + Vector3.Multiply(_v, randomPoint.Y);
        
        return new(
            _origin + offset, 
            _lowerLeftCorner + Vector3.Multiply(_horizontal, u) + Vector3.Multiply(_vertical, v) - _origin - offset);
    }
}