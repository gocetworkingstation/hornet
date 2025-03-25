using HS.Core.Configuration;
using HS.Core.DTOs;
using HS.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace HS.HashApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HashesController(IHashGenerationService hashService, IMessagePublisher publisher, IHashRepository repository, IMemoryCache cache, IOptions<CachingOptions> cachingOptions)
    : ControllerBase
{
    private readonly string _cacheKey = cachingOptions.Value.CacheKey;
    
    [HttpPost]
    public async Task<IActionResult> Post([FromQuery] int count = 40000)
    {
        var hashes = hashService.GenerateHashes(count);
        await publisher.PublishHashesAsync(hashes);
        cache.Remove(_cacheKey); 
        Console.WriteLine($"Cache flushed after POST at {DateTime.Now}");
        return Ok(new { HashesGenerated = hashes.Length });
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        if (!cache.TryGetValue(_cacheKey, out IEnumerable<HashCountDto>? hashCounts))
        {
            hashCounts = await repository.GetHashCountsByDayAsync();
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cachingOptions.Value.HashCountsExpirationMinutes)
            };
            cache.Set(_cacheKey, hashCounts, cacheOptions);
            Console.WriteLine($"Fetched from DB and cached for {cachingOptions.Value.HashCountsExpirationMinutes} minutes at {DateTime.Now}");
        }
        else
        {
            Console.WriteLine($"Retrieved from cache at {DateTime.Now}");
        }
        return Ok(hashCounts);
    }
}