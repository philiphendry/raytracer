namespace RayTracer.SceneBuilder;

public interface IObjectBuilder
{
    IHittable Build();
}