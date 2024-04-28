using PomaPlayer.CurrencyRates.Cron.DtoModels;
using PomaPlayer.CurrencyRates.Cron.Interfaces.Services;
using PomaPlayer.CurrencyRates.Cron.Refit;
using System.Globalization;

namespace PomaPlayer.CurrencyRates.Cron.Services;

public sealed class CronService : ICronService
{
    private readonly IApiCronService _apiCronService;

    public CronService(IApiCronService apiCronService)
    {
        _apiCronService = apiCronService;
    }

    public async Task<ReportDailyDto> GetReportsAsync(DateOnly date, CancellationToken cancellationToken)
    {
        var report = await _apiCronService.GetDailyReport(date.ToString(), cancellationToken);
        return Parse(report);
    }

    private static ReportDailyDto Parse(string report)
    {
        var dateSpan = report.AsSpan(0, report.IndexOf('#') - 1);
        var date = DateOnly.Parse(dateSpan);

        var reportSpan = report.AsSpan(report.IndexOf('\n') + 1);
        reportSpan = reportSpan[(reportSpan.IndexOf('\n') + 1)..];

        var result = new List<ReportDataDto>();
        while (!reportSpan.IsEmpty)
        {
            var data = reportSpan[..reportSpan.IndexOf('\n')];
            var country = data[..data.IndexOf('|')];

            data = data[(data.IndexOf('|') + 1)..];
            var currency = data[..data.IndexOf('|')];

            data = data[(data.IndexOf('|') + 1)..];
            var amount = int.Parse(data[..data.IndexOf('|')]);

            data = data[(data.IndexOf('|') + 1)..];
            var code = data[..data.IndexOf('|')];

            data = data[(data.IndexOf('|') + 1)..];
            var rate = decimal.Parse(data, NumberStyles.Any, CultureInfo.InvariantCulture);

            result.Add(new ReportDataDto()
            {
                Amount = amount,
                Code = new string(code),
                Rate = rate / amount,
                Currency = new string(currency),
                Country = new string(country)
            });

            reportSpan = reportSpan[(reportSpan.IndexOf('\n') + 1)..];
        }

        return new ReportDailyDto
        {
            Date = date,
            Reports = result.ToArray()
        };
    }
}
