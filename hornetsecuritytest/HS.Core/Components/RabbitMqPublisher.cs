using System.Diagnostics;
using System.Text;
using HS.Core.Interfaces;
using RabbitMQ.Client;

namespace HS.Core.Components
{
    public class RabbitMqPublisher(ConnectionFactory factory) : IMessagePublisher
    {
        public async Task PublishHashesAsync(string[] hashes, int batchSize = 1000, int maxConcurrency = 100)
        {
            if (hashes == null || hashes.Length == 0) return;

            var stopwatch = Stopwatch.StartNew();

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync("hashQueue", false, false, false);

            var semaphore = new SemaphoreSlim(maxConcurrency, maxConcurrency);
            var tasks = new List<Task>();

            for (int i = 0; i < hashes.Length; i += batchSize)
            {
                var batch = hashes.Skip(i).Take(batchSize).ToArray();
                tasks.Add(Task.Run(async () =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        var batchStopwatch = Stopwatch.StartNew();
                        foreach (var hash in batch)
                        {
                            var body = Encoding.UTF8.GetBytes(hash);
                            await channel.BasicPublishAsync("", "hashQueue", body);
                        }

                        batchStopwatch.Stop();
                        Console.WriteLine(
                            $"Published batch of {batch.Length} hashes in {batchStopwatch.ElapsedMilliseconds} ms on Thread ID: {Thread.CurrentThread.ManagedThreadId}");
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            await Task.WhenAll(tasks);
            stopwatch.Stop();
            Console.WriteLine(
                $"Published {hashes.Length} hashes to RabbitMQ in {stopwatch.ElapsedMilliseconds} ms with {maxConcurrency} concurrent batches");
        }
    }
}