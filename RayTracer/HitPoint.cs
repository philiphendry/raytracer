﻿using System.Numerics;
using RayTracer.Materials;

namespace RayTracer;

public struct HitPoint
{
    public IMaterial Material { get; }
    public Vector3 Point { get; }
    public Vector3 Normal { get; }
    public float T { get; }
    public bool FrontFace { get; }

    public HitPoint(Ray ray, float t, Vector3 outwardNormal, IMaterial material)
    {
        Point = ray.PositionAt(t);
        T = t;
        FrontFace = Vector3.Dot(ray.Direction, outwardNormal) < 0;
        Normal = FrontFace ? outwardNormal : -outwardNormal;
        Material = material;
    }
}