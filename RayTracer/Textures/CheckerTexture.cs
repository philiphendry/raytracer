using System.Numerics;

namespace RayTracer.Textures;

public class CheckerTexture : Texture
{
    private readonly Texture _even;
    private readonly Texture _odd;

    public CheckerTexture(Texture even, Texture odd)
    {
        _even = even;
        _odd = odd;
    }

    public CheckerTexture(Vector3 evenColour, Vector3 oddColour) 
        : this(new SolidColour(evenColour), new SolidColour(oddColour))
    {
    }

    public override Vector3 Value(float u, float v, Vector3 p)
    {
        var sines = (float)(Math.Sin(10 * p.X) * Math.Sin(10 * p.Y) * Math.Sin(10 * p.Z));
        return sines < 0 ? _odd.Value(u, v, p) : _even.Value(u, v, p);
    }
}