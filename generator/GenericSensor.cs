using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;

namespace generator
{
    class GenericSensor
    {
        private Random random;
        private string queueName;
        private IModel channel;
        private int min;
        private int max;
        private int timeout;
        public string SensorId {get; private set;}
        public Boolean Started {get; set;}

        public GenericSensor(IModel channel, IConfigurationSection configSection, string sensorId)
        {
            getConfigVars(configSection);
            random = new Random();
            this.channel = channel;
            this.channel.QueueDeclare(queueName, false, false, false, null);
            SensorId = sensorId;
        }

        private void getConfigVars(IConfigurationSection config)
        {
            min = config.GetValue<Int32>("Min");
            max = config.GetValue<Int32>("Max");
            int numerator = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;
            int divisor = config.GetValue<Int32>("HowManyDataPerMinute");
            timeout = numerator / divisor;
            queueName = config.GetValue<String>("QueueName");
        }

        public Thread publishingThread() {
            var publishingThread = new Thread(() =>
            {
                while (Started)
                {
                    var message = new Message()
                    {
                        MeasureTime = DateTime.Now,
                        MeasureValue = random.Next(min, max),
                        SensorId = SensorId
                    };
                    var jsonMessage = JsonSerializer.Serialize(message);
                    Thread.Sleep(timeout);
                    Console.WriteLine($"{queueName.ToUpper()}: {message}");
                    channel.BasicPublish("", queueName, null, Encoding.UTF8.GetBytes(jsonMessage));
                }
            });
            return publishingThread;
        }

        public override string ToString()
        {
            return $"QueueName: {queueName} " +
                $"Min: {min} " +
                $"Max: {max} " +
                $"Timeout: {timeout}";
        }
    }
}
