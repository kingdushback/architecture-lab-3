using PomaPlayer.CurrencyRates.Cron.DtoModels;

namespace PomaPlayer.CurrencyRates.Cron.Interfaces.Services;

public interface ICronService
{
    Task<ReportDailyDto> GetReportsAsync(DateOnly date, CancellationToken cancellationToken);
}
