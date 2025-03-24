using HS.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HS.HashApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HashesController(IHashGenerationService hashService, IMessagePublisher publisher, IHashRepository repository)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromQuery] int count = 40000)
    {
        var hashes = hashService.GenerateHashes(count);
        await publisher.PublishHashesAsync(hashes);
        return Ok($"Generated and published {count} hashes");
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var hashes = await repository.GetHashCountsByDayAsync();
        return Ok(new { hashes });
    }
}