using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;

namespace generator
{
    class TemperatureSensor
    {
        private Random random;
        private string queueName;
        private IModel channel;
        private int min;
        private int max;
        private int timeout;

        public TemperatureSensor(IModel channel) 
        {
            random = new Random();
            queueName = "temperature_sensor";
            this.channel = channel;
            this.channel.QueueDeclare(queueName, false, false, false, null);
            getConfigVars();
        }

        private void getConfigVars() 
        {
            IConfiguration config = new ConfigurationBuilder()
                   .AddJsonFile($"appsettings.json", false, true)
                   .Build();
            min = config.GetValue<Int32>("MIN_TEMPERATURE");
            max = config.GetValue<Int32>("MAX_TEMPERATURE");
            int numerator = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;
            int divisor = config.GetValue<Int32>("HOW_MANY_DATA_PER_MINUTE_FOR_TEMPERATURE");
            timeout = numerator / divisor;
        }

        public void publish() {
            int x = random.Next(min, max);
            System.Threading.Thread.Sleep(timeout);
            Console.WriteLine($"Publishing generated value: {x}");
            channel.BasicPublish("", queueName, null, BitConverter.GetBytes(x));
        }
    }
}
