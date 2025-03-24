namespace HS.Core.Interfaces;

public interface IMessageConsumer: IDisposable
{
    Task StartConsumingAsync(Func<string, Task> processMessage);
}
