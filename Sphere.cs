using System.Numerics;
using RayTracer.Materials;

namespace RayTracer;

public class Sphere : IHittable
{
    private readonly bool _enableHitCounts;

    public Vector3 Origin { get; }
    public float Radius { get; }
    public IMaterial Material { get; }

    public int _hitCount;

    public Sphere(Vector3 origin, float radius, IMaterial material, bool enableHitCounts)
    {
        _enableHitCounts = enableHitCounts;
        Origin = origin;
        Radius = radius;
        Material = material;
    }

    public HitPoint? Hit(Ray ray, float tMin, float tMax)
    {
        if (_enableHitCounts)
            Interlocked.Increment(ref _hitCount);

        // The equation of a sphere is (x - Cx)^2 + (y - Cy)^2 + (z - Cz)^2 = R^2
        // The vector from the center C to point P is (P - C) which means the equation above can be written (P - C) dot (P - C) = R^2
        // We are trying to calculate if our ray hits the sphere for a value of t so given the ray equation of P(t) = A + tb
        // We can re-write our sphere equation as (A + tb - C) dot (A + tb - C) = R^2. Where a value of t makes that equation true the ray hits the sphere.
        // Expanding and re-arranging gives us t(b dot b) + 2tb dot (A - C) + (A - C) dot (A - C) - r^2 = 0
        // Which is a quadratic in the form at^2 + bt + c = 0. The quadratic formula is t = (-b +- sqrt(b^2 - 4ac)) / 2a or t = (-b +- sqrt(discriminant)) / 2a
        var oc = ray.Origin - Origin; // (A - C)
        //var a = Vector3.Dot(ray.Direction, ray.Direction); // (b dot b)
        //var b = 2.0f * Vector3.Dot(oc, ray.Direction); // 2b dot (A - C)
        //var c = Vector3.Dot(oc, oc) - radius * radius; // (A - C) dot (A - C) - r^2

        // (b dot b) is equivalent to the squared length of b or b = 2h.
        // Replacing b with 2h in the quadratic formula give (-2h +- sqrt((2h)^2 - 4ac) / 2a which simplifies to (-h +- sqrt(h^2 - ac)) / a
        var a = ray.Direction.LengthSquared();
        var halfB = Vector3.Dot(oc, ray.Direction);
        var c = oc.LengthSquared() - Radius * Radius;

        // The discriminant allows us to figure out if the quadratic equation is solvable.
        // i.e. positive means two solutions, zero just one and negative means the solutions result in complex numbers.
        //var discriminant = b * b - 4.0f * a * c;
        var discriminant = halfB * halfB - a * c;
        var sqrtDiscriminant = (float)Math.Sqrt(discriminant);

        if (discriminant < 0)
            return null;

        // Find the closest root that lies in the tMin/tMax range		
        var root = (-halfB - sqrtDiscriminant) / a;
        if (root < tMin || root > tMax)
        {
            root = (-halfB + sqrtDiscriminant) / a;
            if (root < tMin || root > tMax)
                return null;
        }

        return new HitPoint(ray, root, (ray.PositionAt(root) - Origin) / Radius, Material);
    }

    public AxisAlignedBoundingBox BoundingBox(/*float time0, float time1*/)
    {
        var boundingBoxCornerOffsetFromOrigin = new Vector3(Radius, Radius, Radius);
        return new AxisAlignedBoundingBox(Origin - boundingBoxCornerOffsetFromOrigin, Origin + boundingBoxCornerOffsetFromOrigin);
    }

    public long HitCount => _hitCount;

    public override string ToString() => $"Origin={Origin}, Radius={Radius}, Bounding={BoundingBox()}";
}