using System.Text;
using HS.Core.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace HS.Core.Components;

public class RabbitMqConsumer(ConnectionFactory factory) : IMessageConsumer
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly SemaphoreSlim _semaphore = new(4, 4);
    private int _activeTasks = 0;

    private async Task EnsureInitializedAsync()
    {
        if (_connection is null || _channel is null)
        {
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            await _channel.QueueDeclareAsync("hashQueue", false, false, false);
            await _channel.BasicQosAsync(0, 4, false);
        }
    }

    public async Task StartConsumingAsync(Func<string, Task> processMessage)
    {
        await EnsureInitializedAsync();
        Console.WriteLine("Starting to consume from hashQueue...");

        if (_channel != null)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                await _semaphore.WaitAsync();
                try
                {
                    var active = Interlocked.Increment(ref _activeTasks);
                    Console.WriteLine($"Processing on Thread ID: {Thread.CurrentThread.ManagedThreadId}, Active Tasks: {active}");
                    var hash = Encoding.UTF8.GetString(ea.Body.ToArray());
                    await processMessage(hash);
                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                finally
                {
                    Interlocked.Decrement(ref _activeTasks);
                    _semaphore.Release();
                }
            };
            await _channel.BasicConsumeAsync("hashQueue", false, consumer);
        }

        Console.WriteLine("Consumption started");
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        _semaphore.Dispose();
    }
}