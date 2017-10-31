using Orleans;
using Orleans.Runtime.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using Prototype.Interfaces.Analytics;
using Prototype.Interfaces.Orders;

namespace TestClient
{
    public class Program
    {
        static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                using (var client = await StartClientWithRetries())
                {
                    await DoClientWork(client);
                    Console.ReadKey();
                }

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 1;
            }
        }

        private static async Task<IClusterClient> StartClientWithRetries(int initializeAttemptsBeforeFailing = 5)
        {
            int attempt = 0;
            IClusterClient client;
            while (true)
            {
                try
                {
                    var config = ClientConfiguration.LocalhostSilo();
                    client = new ClientBuilder()
                        .UseConfiguration(config)
                        .AddApplicationPartsFromReferences(typeof(IOrderGrain).Assembly)
                        .ConfigureLogging(logging => logging.AddConsole())
                        .Build();

                    await client.Connect();
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
                    await Task.Delay(TimeSpan.FromSeconds(4));
                }
            }

            return client;
        }

        private static async Task DoClientWork(IClusterClient client)
        {
            // example of calling grains from the initialized client
            var order = client.GetGrain<IOrderGrain>(0);
            var response = await order.GetState().ConfigureAwait(false);
            Console.WriteLine("\n\n{0}\n\n", response);

            var analyticsGrain = client.GetGrain<IRealtimeAnalyticsGrain>(0);
            await analyticsGrain.TrackAction("Some action");
            await Task.Delay(TimeSpan.FromSeconds(30));
            await analyticsGrain.TrackAction("Another action");
            await analyticsGrain.TrackAction("Third action");
        }

        private static async Task DoLoadWork(IClusterClient client)
        {
            var tasks = new List<Task>();

            var offerRandom = new Random(854332554);
            var countRandom = new Random(1687865657);

            for (int i = 1; i < 100; i++)
            {
                var order = client.GetGrain<IOrderGrain>(i);
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
                            var item = new OrderItem { Id = itemId, Quantity = 1, Sku = "123"};
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

            try
            {
                await Task.WhenAll(tasks);
                Console.WriteLine("All orders processing finished.");
            }
            catch (Exception exception)
            {
                
            }
        }
    }
}
