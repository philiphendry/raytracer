namespace RayTracer.Scenes;

public interface ISceneGenerator
{
    IEnumerable<IHittable> Build();
}