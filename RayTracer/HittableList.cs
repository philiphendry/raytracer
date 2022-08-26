namespace RayTracer;

public class HittableList : IHittable
{
    private readonly IEnumerable<IHittable> _hittables;
    private readonly AxisAlignedBoundingBox? _boundingBox;
    private readonly bool _enableHitCounts;
    private int _hitCount;

    public HittableList(IEnumerable<IHittable> hittables, CommandLineOptions options)
    {
        _hittables = hittables;
        _enableHitCounts = options.EnabledHitCounts;
        _boundingBox = CalculateBoundingBox();
    }

    public HitPoint? Hit(Ray ray, float tMin, float tMax)
    {
        if (_enableHitCounts)
            Interlocked.Increment(ref _hitCount);

        HitPoint? hitPoint = null;
        var closestT = tMax;

        foreach (var hittable in _hittables)
        {
            var nextHitPoint = hittable.Hit(ray, tMin, closestT);
            if (nextHitPoint.HasValue)
            {
                hitPoint = nextHitPoint;
                closestT = nextHitPoint.Value.T;
            }
        }

        return hitPoint;
    }

    public AxisAlignedBoundingBox? BoundingBox() => _boundingBox;

    public long HitCount => _hitCount;

    public void DisplayHitCounts(int depth = 0) => Console.WriteLine($"{new string(' ', depth * 2)}{GetType().Name} : {_hitCount:n0}");

    private AxisAlignedBoundingBox? CalculateBoundingBox()
    {
        if (!_hittables.Any())
            return null;

        var firstBox = true;
        AxisAlignedBoundingBox? outputBox = null;
        foreach (var hittable in _hittables)
        {
            var boundingBox = hittable.BoundingBox();
            outputBox = firstBox ? boundingBox : AxisAlignedBoundingBox.CreateSurrounding(outputBox!, boundingBox!);
            firstBox = false;
        }

        return outputBox;
    }
}