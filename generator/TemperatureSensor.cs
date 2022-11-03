using RabbitMQ.Client;
using System;
using System.Configuration;

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
            this.random = new Random();
            this.queueName = "temperature_sensor";
            this.min = Int32.Parse(ConfigurationManager.AppSettings["MIN_TEMPERATURE"]);
            this.max = Int32.Parse(ConfigurationManager.AppSettings["MAX_TEMPERATURE"]);
            int numerator = Int32.Parse(ConfigurationManager.AppSettings["MINUTE_IN_MILISECONDS"]); 
            int divisor = Int32.Parse(ConfigurationManager.AppSettings["HOW_MANY_DATA_PER_MINUTE_FOR_TEMPERATURE"]);
            this.timeout = numerator / divisor;
            this.channel = channel;
            this.channel.QueueDeclare(queueName, false, false, false, null);
        }

        public void publish() {
            int x = random.Next(min, max);
            System.Threading.Thread.Sleep(timeout);
            channel.BasicPublish("", queueName, null, BitConverter.GetBytes(x));
        }
    }
}
