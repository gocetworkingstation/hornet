using HS.Core.Interfaces;
using Microsoft.Extensions.Hosting;

namespace HS.HashProcessor;

public class HashProcessorWorker(IMessageConsumer consumer, IHashRepository repository) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Processor starting...");
        await consumer.StartConsumingAsync(async hash =>
        {
            await repository.SaveHashAsync(hash);
            Console.WriteLine($"Saved hash: {hash}");
        });
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}