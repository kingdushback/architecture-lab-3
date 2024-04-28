using PomaPlayer.CurrencyRates.Cron.DtoModels;
using PomaPlayer.CurrencyRates.Storage.Database;

namespace PomaPlayer.CurrencyRates.Logic.Interfaces.Repositories;

public interface IRepository
{
    Task<ReportDto[]> GetReportAsync(DataContext dataContext, DateOnly start, DateOnly end, CancellationToken cancellationToken = default);

    Task SaveReportsAsync(DataContext dataContext, ReportDailyDto report, CancellationToken cancellationToken = default);
}
