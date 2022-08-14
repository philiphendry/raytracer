using System.Numerics;
using RayTracer.Scenes;

namespace RayTracer;

public static class Utility
{
    public static float Random() => (float)System.Random.Shared.NextDouble();

    public static float Random(float min, float max) => min + (max - min) * (float)new Random().NextDouble();

    public static float DegreesToRadians(float degrees) => degrees * Constants.PI / 180.0f;

    /// <summary>
    /// Given a range of 1 to totalCount this function returns the range chunked into
    /// ranges of size given by chunkSize. ie. a totalCount of 10 and a chunkSize of 3
    /// return (1,3),(4,6),(7,9),(10,10)
    /// </summary>
    /// <param name="totalCount"></param>
    /// <param name="chunkSize"></param>
    /// <returns></returns>
    public static (int, int)[] ChunkedRange(int totalCount, int chunkSize)
    {
        if (chunkSize < 1)
            throw new ArgumentOutOfRangeException(nameof(chunkSize),
                $"{nameof(chunkSize)} must be greater than or equal to 1.");

        if (chunkSize > totalCount)
            chunkSize = totalCount;

        var chunkCount = totalCount / chunkSize + (totalCount % chunkSize == 0 ? 0 : 1);
        return Enumerable
            .Repeat(1, chunkCount)
            .Select((value, index) => (index * chunkSize + 1, index == totalCount / chunkSize ? totalCount : (index + 1) * chunkSize))
            .ToArray();
    }

    /// <summary>
    /// Takes a collection of tasks and completes the returned task when all tasks have completed. If completion
    /// takes a while a progress lambda is called where all tasks can be observed for their status.
    /// </summary>
    /// <param name="tasks"></param>
    /// <param name="reportProgressAction"></param>
    /// <returns></returns>
    public static async Task WhenAllEx(List<Task<List<(int, int, Vector3)>>> tasks, Action<List<Task<List<(int, int, Vector3)>>>> reportProgressAction)
    {
        // get Task which completes when all 'tasks' have completed
        var whenAllTask = Task.WhenAll(tasks);
        for (; ; )
        {
            // get Task which completes after 250ms
            var timer = Task.Delay(250); // you might want to make this configurable
            // Wait until either all tasks have completed OR 250ms passed
            await Task.WhenAny(whenAllTask, timer);
            // if all tasks have completed, complete the returned task
            if (whenAllTask.IsCompleted)
            {
                return;
            }
            // Otherwise call progress report lambda and do another round
            reportProgressAction(tasks);
        }
    }

    public static ISceneGenerator? GetSceneGenerator(string? sceneName)
    {
        var sceneTypeName = $"{typeof(ISceneGenerator).Namespace}.{sceneName}";
        var sceneType = typeof(ISceneGenerator).Assembly.GetType(sceneTypeName);
        if (sceneType == null)
            return null;

        return Activator.CreateInstance(sceneType) as ISceneGenerator;
    }
}