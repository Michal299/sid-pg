using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace generator
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                UserName = "admin",
                Password = "admin",
                HostName = "SI_175132_rabbitmq",
                Port = 5672
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile($"appsettings.json", false, true)
                    .Build();
                var temperatureSensor = new GenericSensor(channel, config.GetSection("TemperatureSensor"));
                while (true)
                {
                    temperatureSensor.publish();
                }
            }
        }
    }
}
