namespace PomaPlayer.CurrencyRates.Quartz.Extensioncs;

public static class ConfigurationExtensions
{
    public static string GetReportDailyCronSchedule(this IConfiguration configuration)
    {
        return configuration.GetValue<string>("Quartz:Jobs:ReportDailyCronSchedule")
            ?? throw new Exception("Quartz:Jobs:ReportDailyCronSchedule not found");
    }
}
