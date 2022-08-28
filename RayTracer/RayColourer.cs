using System.Numerics;
using RayTracer.Objects;
using RayTracer.Utilities;

namespace RayTracer;

public class RayColourer
{
    private readonly bool _useNormalMaterial;
    private readonly bool _disableMaterials;
    private readonly bool _disableLambertian;

    public RayColourer(bool useNormalMaterial, bool disableMaterials, bool disableLambertian)
    {
        _disableLambertian = disableLambertian;
        _disableMaterials = disableMaterials;
        _useNormalMaterial = useNormalMaterial;
    }

    public Vector3 RayColour(Ray ray, World world, int depth)
    {
        if (world == null) throw new ArgumentNullException(nameof(world));

        // Reached max depth so don't collect any more light
        if (depth <= 0)
            return new Vector3(0.0f, 0.0f, 0.0f);

        var hitPoint = world.Hit(ray, 0.001f, float.PositiveInfinity);
        if (!hitPoint.HasValue)
        {
            return world.Background.GetColour(ray);
        }

        var hp = hitPoint.Value;
        if (_useNormalMaterial)
        {
            // Because we have a unit normal we can convert to a colour
            return Vector3.Multiply(
                new Vector3(
                    hp.Normal.X + 1.0f, 
                    hp.Normal.Y + 1.0f,
                    hp.Normal.Z + 1.0f), 
                0.5f);
        }

        if (_disableMaterials)
        {
            var target = _disableLambertian
                ? Vector3Utility.RandomInHemisphere(hp.Normal)
                : hp.Normal + Vector3Utility.RandomUnitVector();

            return 0.5f * RayColour(new Ray(hp.Point, target), world, depth - 1);
        }

        var emitted = hp.Material.Emitted(hp.U, hp.V, hp.Point);
        var scatterResult = hp.Material.Scatter(ray, hp);

        return scatterResult == null
            ? emitted
            : emitted + scatterResult.Value.attenuation * RayColour(scatterResult.Value.scatteredRay, world, depth - 1);
    }
}