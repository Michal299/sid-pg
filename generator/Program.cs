using RabbitMQ.Client;
using System;
using System.Text;

namespace generator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string queueName = "sensor1";
            var factory = new ConnectionFactory()
            {
                UserName = "admin",
                Password = "admin",
                HostName = "rabbitmq",
                Port = 5672
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queueName, false, false, false, null);

                for (int i = 1; i <= 10; i++)
                {
                    var body = Encoding.UTF8.GetBytes($"message{i}");
                    channel.BasicPublish("", queueName, null, body);
                }
            }
        }
    }
}
