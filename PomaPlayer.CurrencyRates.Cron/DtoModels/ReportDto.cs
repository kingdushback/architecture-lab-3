namespace PomaPlayer.CurrencyRates.Cron.DtoModels;

public sealed record ReportDto
{
    public string Code { get; init; }

    public decimal Avg { get; init; }

    public decimal Max { get; init; }

    public decimal Min { get; init; }
}
