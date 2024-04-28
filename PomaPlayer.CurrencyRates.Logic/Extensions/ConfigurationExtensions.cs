using Microsoft.Extensions.Configuration;

namespace PomaPlayer.CurrencyRates.Logic.Extensions;

public static class ConfigurationExtensions
{
    public static string GetDefaultConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("DefaultConnection")
            ?? throw new Exception("DefaultConnection not found");
    }
}
