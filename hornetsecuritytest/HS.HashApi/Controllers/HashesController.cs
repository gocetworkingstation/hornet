using Dapper;
using HS.Core.Configuration;
using HS.Core.DTOs;
using HS.Core.Interfaces;
using HS.Core.Services;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace HS.HashApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HashesController : ControllerBase
{
    private readonly IHashGenerationService _hashService;
    private readonly IMessagePublisher _publisher;

    public HashesController(IHashGenerationService hashService, IMessagePublisher publisher)
    {
        _hashService = hashService;
        _publisher = publisher;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromQuery] int count = 40000)
    {
        var hashes = _hashService.GenerateHashes(count);
        await _publisher.PublishHashesAsync(hashes);
        return Ok($"Generated and published {count} hashes, check console for timing");
    }

    [HttpGet]
    public IActionResult Get()
    {
        using var connection = new MySqlConnection(DbConfig.ConnectionString);
        var hashes = connection.Query("SELECT DATE(Date) as Date, COUNT(*) as Count FROM hashes GROUP BY DATE(Date)")
            .Select(row => new HashCountDto
            {
                Date = row.Date.ToString("yyyy-MM-dd"),
                Count = (long)row.Count
            });
        return Ok(new { hashes });
    }
}