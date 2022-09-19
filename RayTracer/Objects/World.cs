using RayTracer.Objects.Backgrounds;

namespace RayTracer.Objects;

public sealed class World : HittableList
{
    public World(IEnumerable<IHittable> worldObjects, IBackground background) : base(worldObjects)
    {
        Background = background ?? throw new ArgumentNullException(nameof(background));
    }

    public IBackground Background { get; set; }
}