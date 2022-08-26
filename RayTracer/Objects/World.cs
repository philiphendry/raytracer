using System.Collections.Immutable;

namespace RayTracer.Objects;

public class World : HittableList
{
    public World(ImmutableArray<IHittable> worldObjects, IBackground background, CommandLineOptions options)
        :base(worldObjects, options)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));

        Background = background ?? throw new ArgumentNullException(nameof(background));
    }

    public ImmutableArray<IHittable> Objects { get; }

    public IBackground Background { get; }
}