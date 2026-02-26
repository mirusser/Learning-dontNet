namespace SynchronizationDemo.SimpleExamples;

public class ProducerConsumerWithExplicitMonitor
{
    public void Run()
    {
        var queue = new ProducerConsumerQueue<int>();

        var producer = new Thread(() =>
        {
            for (int i = 1; i <= 5; i++)
            {
                Thread.Sleep(500);
                queue.Enqueue(i);
            }
        });

        var consumer = new Thread(() =>
        {
            for (int i = 1; i <= 5; i++)
            {
                Thread.Sleep(1000);
                queue.Dequeue();
            }
        });

        consumer.Start();
        producer.Start();

        producer.Join();
        consumer.Join();
    }
}

// explicit Monitor.Enter/Exit
public class ProducerConsumerQueue<T>
{
    private readonly Queue<T> queue = new ();
    private readonly object @lock = new ();

    public void Enqueue(T item)
    {
        bool lockTaken = false;
        try
        {
            Monitor.Enter(@lock, ref lockTaken);

            queue.Enqueue(item);
            Console.WriteLine($"Produced: {item}");

            // Wake one waiting consumer
            Monitor.Pulse(@lock);
        }
        finally
        {
            if (lockTaken)
            {
                Monitor.Exit(@lock);
            }
        }
    }

    public T Dequeue()
    {
        bool lockTaken = false;
        try
        {
            Monitor.Enter(@lock, ref lockTaken);

            while (queue.Count == 0)
            {
                Console.WriteLine("Queue empty, consumer waiting...");
                Monitor.Wait(@lock);
            }

            var item = queue.Dequeue();
            Console.WriteLine($"Consumed: {item}");
            return item;
        }
        finally
        {
            if (lockTaken)
            {
                Monitor.Exit(@lock);
            }
        }
    }
}