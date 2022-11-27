using System.Collections.Concurrent;

namespace SandBox.TaskAPI
{
    public class UltimateExample
    {
        public void Exectue(int numThreads = 4, int numItemsPerThread = 2, ConcurrentStack<int> stack = null)
        {
            Task.WaitAll(Enumerable
                .Range(0, numThreads)
                .Select(i => Task.Factory.StartNew((obj) =>
                {
                    int index = (int)obj;
                    int[] array = new int[numItemsPerThread];
                    for (int j = 0; j < numItemsPerThread; j++)
                    {
                        array[j] = index + j;
                    }
                    stack.PushRange(array);
                },
                i * numItemsPerThread,
                CancellationToken.None,
                TaskCreationOptions.DenyChildAttach,
                TaskScheduler.Default))
                .ToArray());
        }
    }
}