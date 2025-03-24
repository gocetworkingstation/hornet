namespace HS.Core.Interfaces;

public interface IHashGenerationService
{
    string[] GenerateHashes(int count);
}