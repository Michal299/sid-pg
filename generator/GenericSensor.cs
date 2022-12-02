using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;

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

        public GenericSensor(IModel channel, IConfigurationSection configSection)
        {
            getConfigVars(configSection);
            random = new Random();
            this.channel = channel;
            this.channel.QueueDeclare(queueName, false, false, false, null);
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

        public void publish() {
            int x = random.Next(min, max);
            System.Threading.Thread.Sleep(timeout);
            Console.WriteLine($"{queueName.ToUpper()}: {x}");
            channel.BasicPublish("", queueName, null, BitConverter.GetBytes(x));
        }
    }
}
