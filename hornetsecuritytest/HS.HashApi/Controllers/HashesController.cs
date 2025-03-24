using Dapper;
using HS.Core.Configuration;
using HS.Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace HS.HashApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HashesController : ControllerBase
{
    private readonly HashGenerationService _hashService = new();

    [HttpPost]
    public IActionResult Post([FromQuery] int count = 40000)
    {
        var hashes = _hashService.GenerateHashes(count);
        return Ok($"Generated {count} hashes");
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