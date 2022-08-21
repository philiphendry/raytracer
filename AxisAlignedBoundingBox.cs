using System.Numerics;

namespace RayTracer;

public class AxisAlignedBoundingBox
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
            //var interval1 = (Minimum.GetAxisByIndex(axisIndex) - ray.Origin.GetAxisByIndex(axisIndex)) / ray.Direction.GetAxisByIndex(axisIndex);
            //var interval2 = (Maximum.GetAxisByIndex(axisIndex) - ray.Origin.GetAxisByIndex(axisIndex)) / ray.Direction.GetAxisByIndex(axisIndex);
            //var min = Math.Max(Math.Min(interval1, interval2), tMin);
            //var max = Math.Min(Math.Max(interval1, interval2), tMax);
            //if (max <= min)
            //    return false;

            var inverseDirection = 1.0f / ray.Direction.GetAxisByIndex(axisIndex);
            var t0 = (Minimum.GetAxisByIndex(axisIndex) - ray.Origin.GetAxisByIndex(axisIndex)) * inverseDirection;
            var t1 = (Maximum.GetAxisByIndex(axisIndex) - ray.Origin.GetAxisByIndex(axisIndex)) * inverseDirection;
            if (inverseDirection < 0.0f)
                (t0, t1) = (t1, t0);

            var min = t0 > tMin ? t0 : tMin;
            var max = t1 < tMax ? t1 : tMax;
            if (max <= min)
                return false;
        }

        return true;
    }
}