using System;
using System.Collections.Generic;
using System.Threading;
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
                List<GenericSensor> sensors = new List<GenericSensor>
                {
                    new GenericSensor(channel, config.GetSection("TemperatureSensor")),
                    new GenericSensor(channel, config.GetSection("NoiseSensor")),
                    new GenericSensor(channel, config.GetSection("MusicVolumeSensor")),
                    new GenericSensor(channel, config.GetSection("PeopleCounterSensor"))
                };
                List<Thread> threads = new List<Thread>();

                foreach (var sensor in sensors)
                {
                    Console.WriteLine(sensor);
                    sensor.Started = true;
                    var thread = sensor.publishingThread();
                    threads.Add(thread);
                    thread.Start();
                }

                foreach (var thread in threads)
                {
                    thread.Join();
                }
            }
        }
    }
}
