using RabbitMQ.Client;
using System;

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
            this.queueName = "sensor1";
            this.channel = channel;
            this.channel.QueueDeclare(queueName, false, false, false, null);
        }

        public void publish() {
            int x = random.Next(1, 100);
            System.Threading.Thread.Sleep(5000);
            channel.BasicPublish("", queueName, null, convertToBytes(x));
        }

        private byte[] convertToBytes(int number) 
        {
            byte[] bytes = BitConverter.GetBytes(number);
            Array.Reverse(bytes);
            return bytes;
        }
    }
}
