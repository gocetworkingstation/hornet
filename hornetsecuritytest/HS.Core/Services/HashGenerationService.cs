using System.Diagnostics;
using System.Security.Cryptography;
using HS.Core.Interfaces;

namespace HS.Core.Services;

public class HashGenerationService : IHashGenerationService
{
    public string[] GenerateHashes(int count = 40000)
    {
        Console.WriteLine($"Starting generation of {count} SHA1 hashes");
        var stopwatch = Stopwatch.StartNew();

        string[] hashes = new string[count];
        Parallel.For(0, count, i =>
        {
            byte[] bytes = new byte[16];
            Random.Shared.NextBytes(bytes);
            var hash = SHA1.HashData(bytes);
            hashes[i] = Convert.ToHexString(hash).ToLower();
        });

        stopwatch.Stop();
        Console.WriteLine($"Generated {count} hashes in {stopwatch.ElapsedMilliseconds} ms");
        return hashes;
    }
}