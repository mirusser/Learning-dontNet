namespace SynchronizationDemo.SimpleExamples;

public class BoundedProducerConsumer
{
    public void Run()
    {
        var queue = new BoundedQueue<int>(capacity: 2);

        var producer = new Thread(() =>
        {
            for (int i = 1; i <= 6; i++)
            {
                Thread.Sleep(200);
                queue.Enqueue(i);
            }
        });

        var consumer = new Thread(() =>
        {
            for (int i = 1; i <= 6; i++)
            {
                Thread.Sleep(1000);
                queue.Dequeue();
            }
        });

        producer.Start();
        consumer.Start();

        producer.Join();
        consumer.Join();
    }
}

// producers must wait if the queue is full
// consumers must wait if the queue is empty
public class BoundedQueue<T>(int capacity)
{
    private readonly Queue<T> queue = new Queue<T>();
    private readonly object gate = new object();

    public void Enqueue(T item)
    {
        lock (gate)
        {
            while (queue.Count >= capacity)
            {
                Console.WriteLine("Queue full, producer waiting...");
                Monitor.Wait(gate);
            }

            queue.Enqueue(item);
            Console.WriteLine($"Produced: {item} (count={queue.Count})");

            // Wake a waiting consumer
            Monitor.PulseAll(gate);
        }
    }

    public T Dequeue()
    {
        lock (gate)
        {
            while (queue.Count == 0)
            {
                Console.WriteLine("Queue empty, consumer waiting...");
                Monitor.Wait(gate);
            }

            var item = queue.Dequeue();
            Console.WriteLine($"Consumed: {item} (count={queue.Count})");

            // Wake a waiting producer
            Monitor.PulseAll(gate);

            return item;
        }
    }
}