using System.Numerics;
using NUnit.Framework;
using RayTracer;

namespace RayTracerTests;

public class Tests
{
    private const float TMin = 0.0001f;
    private const float TMax = float.PositiveInfinity;

    [Test]
    public void TestThatARayDoesNotHitXDirection() => Assert.IsFalse(
        new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(1, 1, 1))
            .Hit(new Ray(origin: new Vector3(-1, -1, -1), direction: new Vector3(1, 0, 0)), TMin, TMax));

    [Test]
    public void TestThatARayDoesNotHitYDirection() => Assert.IsFalse(
        new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(1, 1, 1))
            .Hit(new Ray(origin: new Vector3(-1, -1, -1), direction: new Vector3(0, 1, 0)), TMin, TMax));

    [Test]
    public void TestThatARayDoesNotHitZDirection() => Assert.IsFalse(
        new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(1, 1, 1))
            .Hit(new Ray(origin: new Vector3(-1, -1, -1), direction: new Vector3(0, 0, 1)), TMin, TMax));

    [Test]
    public void TestThatARayDoesHitInDirectionX() => Assert.IsTrue(
        new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(1, 1, 1))
            .Hit(new Ray(origin: new Vector3(-1.0f, 0.5f, 0.5f), direction: new Vector3(1, 0, 0)), TMin, TMax));

    [Test]
    public void TestThatARayDoesNotHitInDirectionXForASmallValueOfTMax() => Assert.IsFalse(
        new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(1, 1, 1))
            .Hit(new Ray(origin: new Vector3(-1.0f, 0.5f, 0.5f), direction: new Vector3(1, 0, 0)), TMin, 0.5f));

    [Test]
    public void TestThatARayDoesHitInDirectionY() => Assert.IsTrue(
        new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(1, 1, 1))
            .Hit(new Ray(origin: new Vector3(0.5f, -1.0f, 0.5f), direction: new Vector3(0, 1, 0)), TMin, TMax));

    [Test]
    public void TestThatARayDoesNotHitInDirectionYForASmallValueOfTMax() => Assert.IsFalse(
        new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(1, 1, 1))
            .Hit(new Ray(origin: new Vector3(0.5f, -1.0f, 0.5f), direction: new Vector3(0, 1, 0)), TMin, 0.5f));

    [Test]
    public void TestThatARayDoesHitInDirectionZ() => Assert.IsTrue(
        new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(1, 1, 1))
            .Hit(new Ray(origin: new Vector3(0.5f, 0.5f, -1.0f), direction: new Vector3(0, 0, 1)), TMin, TMax));

    [Test]
    public void TestThatARayDoesNotHitInDirectionZForASmallValueOfTMax() => Assert.IsFalse(
        new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(1, 1, 1))
            .Hit(new Ray(origin: new Vector3(0.5f, 0.5f, -1.0f), direction: new Vector3(0, 0, 1)), TMin, 0.5f));

    [Test]
    public void TestThatARayDoesHitInAllDirections() => Assert.IsTrue(
        new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(1, 1, 1))
            .Hit(new Ray(origin: new Vector3(-1.0f, -1.0f, -1.0f), direction: new Vector3(1, 1, 1)), TMin, TMax));

    [Test]
    public void TestThatARayDoesNotHitInAllDirectionsOriginNegativeX() => Assert.IsFalse(
        new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(1, 1, 1))
            .Hit(new Ray(origin: new Vector3(-10.0f, 0, 0), direction: new Vector3(1, 1, 1)), TMin, TMax));

    [Test]
    public void TestThatARayDoesNotHitInAllDirectionsOriginPositiveX() => Assert.IsFalse(
        new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(1, 1, 1))
            .Hit(new Ray(origin: new Vector3(10.0f, 0, 0), direction: new Vector3(1, 1, 1)), TMin, TMax));

    [Test]
    public void TestThatARayDoesNotHitInAllDirectionsOriginNegativeY() => Assert.IsFalse(
        new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(1, 1, 1))
            .Hit(new Ray(origin: new Vector3(0, -10.0f, 0), direction: new Vector3(1, 1, 1)), TMin, TMax));

    [Test]
    public void TestThatARayDoesNotHitInAllDirectionsOriginPositiveY() => Assert.IsFalse(
        new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(1, 1, 1))
            .Hit(new Ray(origin: new Vector3(0, 10.0f, 0), direction: new Vector3(1, 1, 1)), TMin, TMax));

    [Test]
    public void TestThatARayDoesNotHitInAllDirectionsOriginNegativeZ() => Assert.IsFalse(
        new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(1, 1, 1))
            .Hit(new Ray(origin: new Vector3(0, 0, -10.0f), direction: new Vector3(1, 1, 1)), TMin, TMax));

    [Test]
    public void TestThatARayDoesNotHitInAllDirectionsOriginPositiveZ() => Assert.IsFalse(
        new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(1, 1, 1))
            .Hit(new Ray(origin: new Vector3(0, 0, 10.0f), direction: new Vector3(1, 1, 1)), TMin, TMax));

}