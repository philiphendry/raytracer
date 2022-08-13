﻿using System.Numerics;

namespace RayTracer;

public class Camera
{
    private float AspectRatio { get; }
    public int ImageWidth { get; init; }
    public int ImageHeight { get; init; }

    private float ViewportHeight { get; } = 2.0f;
    private float ViewportWidth { get; }
    private float FocalLength { get; } = 1.0f;

    private Vector3 Origin { get; } = Vector3.Zero;
    private Vector3 Horizontal { get; }
    private Vector3 Vertical { get; }
    private Vector3 LowerLeftCorner { get; }

    public Camera(int imageWidth, float aspectRatio)
    {
        AspectRatio = aspectRatio;
        ImageWidth = imageWidth;
        ImageHeight = Convert.ToInt32(imageWidth / aspectRatio);

        ViewportWidth = aspectRatio * ViewportHeight;
        Horizontal = new Vector3(ViewportWidth, 0, 0);
        Vertical = new Vector3(0, ViewportHeight, 0);
        LowerLeftCorner = Origin - Vector3.Divide(Horizontal, 2f) - Vector3.Divide(Vertical, 2f) - new Vector3(0, 0, FocalLength);
    }

    public Ray GetRay(float u, float v)
        => new(Origin, LowerLeftCorner + Vector3.Multiply(Horizontal, u) + Vector3.Multiply(Vertical, v) - Origin);
}