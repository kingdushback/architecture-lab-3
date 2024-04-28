namespace PomaPlayer.CurrencyRates.Cron.DtoModels;

public sealed record ReportDataDto
{
    public string Country { get; init; }

    public string Currency { get; init; }

    public int Amount { get; init; }

    public string Code { get; init; }

    public decimal Rate { get; init; }
}
