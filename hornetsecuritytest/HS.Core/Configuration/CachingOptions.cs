namespace HS.Core.Configuration;

public class CachingOptions
{
    public int HashCountsExpirationMinutes { get; set; }
    public string CacheKey { get; set; }
}