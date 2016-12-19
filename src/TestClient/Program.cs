using Orleans;
using Orleans.Runtime.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Orleans.Runtime;
using Prototype.Interfaces;
using Prototype.Interfaces.Orders;

namespace TestClient
{
    public class Program
    {
        static int Main(string[] args)
        {
            var config = ClientConfiguration.LocalhostSilo();
            try
            {
                //InitializeWithRetries(config, initializeAttemptsBeforeFailing: 5);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Orleans client initialization failed failed due to {ex}");

                Console.ReadLine();
                return 1;
            }

            //DoClientWork().Wait();
            DoLoadWork().Wait();
            Console.WriteLine("Press Enter to terminate...");
            Console.ReadLine();
            return 0;
        }

        private static void InitializeWithRetries(ClientConfiguration config, int initializeAttemptsBeforeFailing)
        {
            int attempt = 0;
            while (true)
            {
                try
                {
                    GrainClient.Initialize(config);
                    Console.WriteLine("Client successfully connect to silo host");
                    break;
                }
                catch (SiloUnavailableException)
                {
                    attempt++;
                    Console.WriteLine($"Attempt {attempt} of {initializeAttemptsBeforeFailing} failed to initialize the Orleans client.");
                    if (attempt > initializeAttemptsBeforeFailing)
                    {
                        throw;
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                }
            }
        }

        private static async Task DoClientWork()
        {
            // example of calling grains from the initialized client
            var order = GrainClient.GrainFactory.GetGrain<IOrderGrain>(0);
            var response = await order.GetState();
            Console.WriteLine("\n\n{0}\n\n", response);
        }

        private static async Task DoLoadWork()
        {
            var tasks = new List<Task>();

            var offerRandom = new Random(123);
            var countRandom = new Random(4984565);

            for (int i = 0; i < 100; i++)
            {
                var order = GrainClient.GrainFactory.GetGrain<IOrderGrain>(i);
                var count = countRandom.Next(1, 5);
                var orderItems = new List<OrderItem>();
                for (int j = 0; j < count; j++)
                {
                    var repeat = true;
                    do
                    {
                        var itemId = offerRandom.Next(1, 10);
                        if (orderItems.All(oi => oi.Id != itemId))
                        {
                            var item = new OrderItem { Id = itemId, Quantity = 1 };
                            orderItems.Add(item);
                            repeat = false;
                        }
                    } while (repeat);
                }

                var createData = new OrderCreateData
                {
                    Items = orderItems
                };

                tasks.Add(order.Create(createData));
            }

            await Task.WhenAll(tasks);
        }
    }
}
