using System.Drawing.Imaging;
using System.Numerics;

namespace RayTracer.Textures;

public sealed class ImageTexture : Texture
{
    private readonly int _width;
    private readonly int _height;
    private readonly byte[] _rgbValues;
    private readonly int _scanlineStride;
    private readonly float _colourScale;
    private const int BytesPerPixel = 3;

    public ImageTexture(string filename)
    {
        if (!File.Exists(filename))
            throw new FileNotFoundException(filename);

        var bitmap = new Bitmap(filename);
        _width = bitmap.Width;
        _height = bitmap.Height;
        _colourScale = 1.0f / 255.0f;

        var bitmapData = bitmap.LockBits(new Rectangle(0, 0, _width, _height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        _scanlineStride = bitmapData.Stride;

        var byteCount = _scanlineStride * _height;
        _rgbValues = new byte[byteCount];

        System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, _rgbValues, 0, byteCount);
        bitmap.UnlockBits(bitmapData);
    }

    public override Vector3 Value(float u, float v, Vector3 p)
    {
        u = Math.Clamp(u, 0.0f, 1.0f);
        v = 1.0f - Math.Clamp(v, 0.0f, 1.0f);

        var i = (int)(u * _width);
        var j = (int)(v * _height);

        if (i >= _width) i = _width - 1;
        if (j >= _height) j = _height - 1;

        var pixel = j * _scanlineStride + i * BytesPerPixel;
        return new Vector3(_colourScale * _rgbValues[pixel + 2], _colourScale * _rgbValues[pixel + 1],_colourScale * _rgbValues[pixel]);
    }
}