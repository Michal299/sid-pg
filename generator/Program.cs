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
                HostName = "rabbitmq",
                Port = 5672
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                TemperatureSensor sensor = new TemperatureSensor(channel);
                while (true)
                {
                    sensor.publish();
                }
            }
        }
    }
}
