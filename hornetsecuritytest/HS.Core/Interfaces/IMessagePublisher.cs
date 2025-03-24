namespace HS.Core.Interfaces;

public interface IMessagePublisher
{
    Task PublishHashesAsync(string[] hashes);
}