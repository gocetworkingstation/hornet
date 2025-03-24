using HS.Core.DTOs;

namespace HS.Core.Interfaces;

public interface IHashRepository
{
    Task SaveHashAsync(string hash);
    Task<IEnumerable<HashCountDto>> GetHashCountsByDayAsync();
}