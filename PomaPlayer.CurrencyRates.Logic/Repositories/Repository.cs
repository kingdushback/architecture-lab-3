using Microsoft.EntityFrameworkCore;
using PomaPlayer.CurrencyRates.Cron.DtoModels;
using PomaPlayer.CurrencyRates.Logic.Exceptions;
using PomaPlayer.CurrencyRates.Logic.Interfaces.Repositories;
using PomaPlayer.CurrencyRates.Storage.Database;
using PomaPlayer.CurrencyRates.Storage.Models;
using System.Net;

namespace PomaPlayer.CurrencyRates.Logic.Repositories;

public sealed class Repository : IRepository
{
    public Repository()
    {

    }

    public async Task<ReportDto[]> GetReportAsync(DataContext dataContext, DateOnly start, DateOnly end, CancellationToken cancellationToken = default)
    {
        if (!dataContext.Reports.Any(x => x.Date == start))
            throw new CurRateException { ErrorCode = "INVALID_QUERY", StatusCode = HttpStatusCode.BadRequest, ErrorDetails = $"Данные за данный период не найдены в БД" };

        if (!dataContext.Reports.Any(x => x.Date == end))
            throw new CurRateException { ErrorCode = "INVALID_QUERY", StatusCode = HttpStatusCode.BadRequest, ErrorDetails = $"Данные за данный период не найдены в БД" };

        return await dataContext.Reports
            .AsNoTracking()
            .Where(x => x.Date >= start && x.Date <= end)
            .GroupBy(x => x.Code)
            .Select(x => new ReportDto
            {
                Code = x.Key,
                Avg = x.Average(e => e.Rate),
                Max = x.Max(e => e.Rate),
                Min = x.Min(e => e.Rate)
            })
            .ToArrayAsync(cancellationToken);
    }

    public async Task SaveReportsAsync(DataContext dataContext, ReportDailyDto report, CancellationToken cancellationToken = default)
    {
        if (!dataContext.Reports.Any(x => x.Date == report.Date))
        {
            var models = MapFromReportDailyDto(report);
            dataContext.Reports.AddRange(models);
        }
    }

    private static ReportDaily[] MapFromReportDailyDto(ReportDailyDto reportDaily)
    {
        var list = new List<ReportDaily>();

        foreach (var report in reportDaily.Reports)
        {
            var model = new ReportDaily
            {
                IsnReportDaily = Guid.NewGuid(),
                Date = reportDaily.Date,
                Country = report.Country,
                Currency = report.Currency,
                Code = report.Code,
                Rate = report.Rate
            };

            list.Add(model);
        }

        return list.ToArray();
    }
}
