using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "taks_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

//settings that tells RabbitMQ to dispatch message to the next worker that is not still busy.
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

Console.WriteLine("Waiting for messages...");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Received: {message}");

    FakeTaskToSimulateExecutionTime(message);

    Console.WriteLine("Done");

    //acknowledging that message was processed (consumed properly)
    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
};

channel.BasicConsume(queue: "task_queue", autoAck: false, consumer: consumer);

Console.ReadKey();

void FakeTaskToSimulateExecutionTime(string message)
{
    int dots = message.Split('.').Length - 1;
    Thread.Sleep(dots * 1000);
}