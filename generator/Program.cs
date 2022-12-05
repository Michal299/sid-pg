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
                    new GenericSensor(channel, config.GetSection("TemperatureSensor"), "ts1"),
                    new GenericSensor(channel, config.GetSection("TemperatureSensor"), "ts2"),
                    new GenericSensor(channel, config.GetSection("NoiseSensor"), "ns1"),
                    new GenericSensor(channel, config.GetSection("NoiseSensor"), "ns2"),
                    new GenericSensor(channel, config.GetSection("NoiseSensor"), "ns3"),
                    new GenericSensor(channel, config.GetSection("MusicVolumeSensor"), "mvs1"),
                    new GenericSensor(channel, config.GetSection("MusicVolumeSensor"), "mvs2"),
                    new GenericSensor(channel, config.GetSection("PeopleCounterSensor"), "pcs1"),
                    new GenericSensor(channel, config.GetSection("PeopleCounterSensor"), "pcs2"),
                    new GenericSensor(channel, config.GetSection("PeopleCounterSensor"), "pcs3")
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
