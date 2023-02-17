using System.Collections.Concurrent;

namespace Application.PrimeNumbers.Extensions;

public static class ConcurrentQueueExtension
{
    public static async IAsyncEnumerable<T> AsAsyncEnumerable<T>(this ConcurrentQueue<T> concurrentQueue)
    {
        await Task.Yield();
        foreach (var p in concurrentQueue.ToArray()) yield return p;
    }
}