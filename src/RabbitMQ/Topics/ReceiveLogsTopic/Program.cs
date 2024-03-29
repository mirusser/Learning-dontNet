﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "topic_logs", type: "topic");
var queueName = channel.QueueDeclare().QueueName;

if(args.Length < 1)
{
    Console.Error.WriteLine($"Usage {Environment.GetCommandLineArgs()[0]} [binding key...]");
    Console.WriteLine("Press [enter] to exit.");
    Environment.ExitCode = 1;
    return;
}

foreach (var bindingKey in args)
{
    channel.QueueBind(queue: queueName, exchange: "topic_logs", routingKey: bindingKey);
}

Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (mode, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var routingKey = ea.RoutingKey;
    Console.WriteLine($" [x] Received {routingKey} : {message}");
};

channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

Console.WriteLine("Press [enter] to exit.");
Console.ReadLine();