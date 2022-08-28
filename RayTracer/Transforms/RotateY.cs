using System.Numerics;
using RayTracer.Utilities;

namespace RayTracer.Transforms;

public class RotateY : IHittable
{
    private readonly IHittable _hittable;
    private readonly float _sinTheta;
    private readonly float _cosTheta;
    private readonly AxisAlignedBoundingBox? _boundingBox;

    public RotateY(IHittable hittable, float angle)
    {
        _hittable = hittable;

        var radians = Utility.DegreesToRadians(angle);
        _sinTheta = MathF.Sin(radians);
        _cosTheta = MathF.Cos(radians);
        _boundingBox = hittable.BoundingBox();

        var min = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
        var max = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

        for (var i = 0; i < 2; i++)
        {
            for (var j = 0; j < 2; j++)
            {
                for (var k = 0; k < 2; k++)
                {
                    var x = i * _boundingBox!.Maximum.X + (1 - i) * _boundingBox.Minimum.X;
                    var y = j * _boundingBox.Maximum.Y + (1 - j) * _boundingBox.Minimum.Y;
                    var z = k * _boundingBox.Maximum.Z + (1 - k) * _boundingBox.Minimum.Z;

                    var newX = _cosTheta * x + _sinTheta * z;
                    var newZ = -_sinTheta * x + _cosTheta * z;

                    min.X = Math.Min(min.X, newX);
                    min.Y = Math.Min(min.Y, y);
                    min.Z = Math.Min(min.Z, newZ);

                    max.X = Math.Max(max.X, newX);
                    max.Y = Math.Max(max.Y, y);
                    max.Z = Math.Max(max.Z, newZ);
                }
            }
        }

        _boundingBox = new AxisAlignedBoundingBox(min, max);
    }

    public HitPoint? Hit(Ray ray, float tMin, float tMax)
    {
        var origin = ray.Origin;
        var direction = ray.Direction;

        origin.X = (float)(_cosTheta * ray.Origin.X - _sinTheta * ray.Origin.Z);
        origin.Z = (float)(_sinTheta * ray.Origin.X + _cosTheta * ray.Origin.Z);

        direction.X = (float)(_cosTheta * ray.Direction.X - _sinTheta * ray.Direction.Z);
        direction.Z = (float)(_sinTheta * ray.Direction.X + _cosTheta * ray.Direction.Z);

        var rotatedRay = new Ray(origin, direction);

        var hitPoint = _hittable.Hit(rotatedRay, tMin, tMax);
        if (hitPoint == null)
            return null;

        var point = hitPoint.Value.Point;
        var normal = hitPoint.Value.Normal;

        point.X = (float)(_cosTheta * hitPoint.Value.Point.X + _sinTheta * hitPoint.Value.Point.Z);
        point.Z = (float)(- _sinTheta * hitPoint.Value.Point.X + _cosTheta * hitPoint.Value.Point.Z);

        normal.X = (float)(_cosTheta * hitPoint.Value.Normal.X + _sinTheta * hitPoint.Value.Normal.Z);
        normal.Z = (float)(-_sinTheta * hitPoint.Value.Normal.X + _cosTheta * hitPoint.Value.Normal.Z);

        return new HitPoint(
            rotatedRay,
            point, 
            hitPoint.Value.T,
            normal,
            hitPoint.Value.Material,
            hitPoint.Value.U,
            hitPoint.Value.V);
    }

    public AxisAlignedBoundingBox? BoundingBox() => _boundingBox;
}