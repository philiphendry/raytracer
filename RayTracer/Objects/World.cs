namespace RayTracer.Objects;

public class World : HittableList
{
    public World(IEnumerable<IHittable> worldObjects, IBackground background) : base(worldObjects)
    {
        Background = background ?? throw new ArgumentNullException(nameof(background));
    }

    public IBackground Background { get; set; }
}