using System.Numerics;
using RayTracer.Utility;

namespace RayTracer;

public sealed class AxisAlignedBoundingBox
{
    public readonly Vector3 Minimum;
    public readonly Vector3 Maximum;

    public AxisAlignedBoundingBox(Vector3 minimum, Vector3 maximum)
    {
        Minimum = minimum;
        Maximum = maximum;
    }

    public static AxisAlignedBoundingBox CreateSurrounding(AxisAlignedBoundingBox box0, AxisAlignedBoundingBox box1)
    {
        var p1 = new Vector3(
            Math.Min(box0.Minimum.X, box1.Minimum.X),
            Math.Min(box0.Minimum.Y, box1.Minimum.Y),
            Math.Min(box0.Minimum.Z, box1.Minimum.Z)
        );
        var p2 = new Vector3(
            Math.Max(box0.Maximum.X, box1.Maximum.X),
            Math.Max(box0.Maximum.Y, box1.Maximum.Y),
            Math.Max(box0.Maximum.Z, box1.Maximum.Z)
        );
        return new AxisAlignedBoundingBox(p1, p2);
    }

    public bool Hit(Ray ray, float tMin, float tMax)
    {
        for (var axisIndex = 0; axisIndex < 3; axisIndex++)
        {
            //var t0 = Math.Min(
            //    (Minimum.GetAxisByIndex(axisIndex) - ray.Origin.GetAxisByIndex(axisIndex)) / ray.Direction.GetAxisByIndex(axisIndex),
            //    (Maximum.GetAxisByIndex(axisIndex) - ray.Origin.GetAxisByIndex(axisIndex)) / ray.Direction.GetAxisByIndex(axisIndex));
            //var t1 = Math.Max(
            //    (Minimum.GetAxisByIndex(axisIndex) - ray.Origin.GetAxisByIndex(axisIndex)) / ray.Direction.GetAxisByIndex(axisIndex),
            //    (Maximum.GetAxisByIndex(axisIndex) - ray.Origin.GetAxisByIndex(axisIndex)) / ray.Direction.GetAxisByIndex(axisIndex));
            //tMin = Math.Max(t0, tMin);
            //tMax = Math.Min(t1, tMax);
            //if (tMax <= tMin)
            //    return false;

            var inverseDirection = 1.0f / ray.Direction.GetAxisByIndex(axisIndex);
            var t0 = (Minimum.GetAxisByIndex(axisIndex) - ray.Origin.GetAxisByIndex(axisIndex)) * inverseDirection;
            var t1 = (Maximum.GetAxisByIndex(axisIndex) - ray.Origin.GetAxisByIndex(axisIndex)) * inverseDirection;
            if (inverseDirection < 0.0f)
                (t0, t1) = (t1, t0);

            tMin = t0 > tMin ? t0 : tMin;
            tMax = t1 < tMax ? t1 : tMax;
            if (tMax <= tMin)
                return false;
        }

        return true;
    }

    public override string ToString() 
        => $"({Minimum.X:n},{Minimum.Y:n},{Minimum.Z:n}) - ({Maximum.X:n},{Maximum.Y:n},{Maximum.Z:n})";
}