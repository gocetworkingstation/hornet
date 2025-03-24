using Dapper;
using HS.Core.Configuration;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace HS.HashApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HashesController : ControllerBase
{
    [HttpPost]
    public IActionResult Post()
    {
        return Ok("POST not implemented yet");
    }

    [HttpGet]
    public IActionResult Get()
    {
        using var connection = new MySqlConnection(DbConfig.ConnectionString);
        var hashes = connection.Query("SELECT DATE(Date) as Date, COUNT(*) as Count FROM hashes GROUP BY DATE(Date)")
            .Select(row => new { date = row.Date.ToString("yyyy-MM-dd"), count = (long)row.Count });
        return Ok(new { hashes });
    }
}