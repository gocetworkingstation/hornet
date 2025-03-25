namespace HS.Core.Interfaces;

public interface IMessagePublisher
{
    Task PublishHashesAsync(string[] hashes, int batchSize = 1000, int maxConcurrency = 4);
}