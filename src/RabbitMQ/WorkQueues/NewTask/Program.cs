using System;
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "hello", durable: true, exclusive: false, autoDelete: false, arguments: null);

var message = GetMessage(args);
var body = Encoding.UTF8.GetBytes(message);
var properties = channel.CreateBasicProperties();
properties.Persistent = true;

channel.BasicPublish(exchange: "", routingKey: "taks_queue", basicProperties: properties, body: body);

static string GetMessage(string[] args)
{
    return ((args.Length > 0) ? string.Join(" ", args) : "Hello world");
}