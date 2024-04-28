using PomaPlayer.CurrencyRates.Cron.DtoModels;
using PomaPlayer.CurrencyRates.Storage.Database;

namespace PomaPlayer.CurrencyRates.Logic.Interfaces.Services;

public interface ICronReportService
{
    Task<ReportDto[]> CalculateReportAsync(DataContext dataContext, DateOnly start, DateOnly end, string[] codes, CancellationToken cancellationToken = default);

    Task SaveReportsAsync(DataContext dataContext, DateOnly start, DateOnly end, CancellationToken cancellationToken = default);
}
