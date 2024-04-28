using PomaPlayer.CurrencyRates.Cron.DtoModels;
using PomaPlayer.CurrencyRates.Cron.Interfaces.Services;
using PomaPlayer.CurrencyRates.Logic.Interfaces.Repositories;
using PomaPlayer.CurrencyRates.Logic.Interfaces.Services;
using PomaPlayer.CurrencyRates.Storage.Database;

namespace PomaPlayer.CurrencyRates.Logic.Services;

public class CronReportService : ICronReportService
{
    private readonly IRepository _repository;
    private readonly ICronService _cronService;

    public CronReportService(IRepository repository, ICronService cronService)
    {
        _repository = repository;
        _cronService = cronService;
    }

    public async Task<ReportDto[]> CalculateReportAsync(DataContext dataContext, DateOnly start, DateOnly end, string[] codes, CancellationToken cancellationToken = default)
    {
        var reports = await _repository.GetReportAsync(dataContext, start, end, cancellationToken);
        return reports.Where(x => codes.Contains(x.Code)).ToArray();
    }

    public async Task SaveReportsAsync(DataContext dataContext, DateOnly start, DateOnly end, CancellationToken cancellationToken = default)
    {
        foreach (var day in GetDays(start, end))
        {
            var report = await _cronService.GetReportsAsync(day, cancellationToken);
            await _repository.SaveReportsAsync(dataContext, report, cancellationToken);
        }
    }

    private IEnumerable<DateOnly> GetDays(DateOnly start, DateOnly end)
    {
        while (start <= end)
        {
            yield return start;
            start = start.AddDays(1);
        }
    }
}
