using System.Diagnostics;
using System.Text;
using HS.Core.Interfaces;
using RabbitMQ.Client;

namespace HS.Core.Components
{
    public class RabbitMqPublisher : IMessagePublisher
    {
        public async Task PublishHashesAsync(string[] hashes)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync(); // Use this
            await channel.QueueDeclareAsync(queue: "hashQueue", durable: false, exclusive: false, autoDelete: false);

            var stopwatch = Stopwatch.StartNew();
            foreach (var hash in hashes)
            {
                var body = Encoding.UTF8.GetBytes(hash);
                await channel.BasicPublishAsync(exchange: "", routingKey: "hashQueue", body: body);
            }
            stopwatch.Stop();
            Console.WriteLine($"Published {hashes.Length} hashes to RabbitMQ in {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}