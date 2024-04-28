namespace PomaPlayer.CurrencyRates.Cron.DtoModels;

public sealed record ReportDailyDto
{
    public DateOnly Date { get; init; }

    public ReportDataDto[] Reports { get; init; }
}
