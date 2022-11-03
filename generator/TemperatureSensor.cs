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

        public TemperatureSensor(IModel channel) 
        {
            this.random = new Random();
            this.queueName = "temperature_sensor";
            this.channel = channel;
            this.channel.QueueDeclare(queueName, false, false, false, null);
        }

        public void publish() {
            int min = Int32.Parse(ConfigurationManager.AppSettings["MIN_TEMPERATURE"]);
            int max = Int32.Parse(ConfigurationManager.AppSettings["MAX_TEMPERATURE"]);
            int timeout = Int32.Parse(ConfigurationManager.AppSettings["MINUTE_IN_MILISECONDS"]) / Int32.Parse(ConfigurationManager.AppSettings["HOW_MANY_DATA_PER_MINUTE_FOR_TEMPERATURE"]);
            int x = random.Next(min, max);
            System.Threading.Thread.Sleep(timeout);
            channel.BasicPublish("", queueName, null, BitConverter.GetBytes(x));
        }
    }
}
