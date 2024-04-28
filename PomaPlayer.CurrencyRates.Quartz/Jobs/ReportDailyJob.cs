using PomaPlayer.CurrencyRates.Logic.Interfaces.Services;
using PomaPlayer.CurrencyRates.Quartz.Jobs.Base;
using PomaPlayer.CurrencyRates.Storage.Database;
using Quartz;

namespace PomaPlayer.CurrencyRates.Quartz.Jobs;

public sealed class ReportDailyJob : BaseJob
{
    public const string JobName = "ReportDaily";
    private readonly ICronReportService _cronReportService;

    public ReportDailyJob(
        ILogger<ReportDailyJob> logger,
        IServiceProvider serviceProvider,
        ICronReportService cronReportService) : base(logger, serviceProvider)
    {
        _cronReportService = cronReportService;
    }

    protected override async Task DoWork(IJobExecutionContext context)
    {
        using var scope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();

        var date = DateOnly.FromDateTime(DateTime.Now);
        await _cronReportService.SaveReportsAsync(dataContext, date, date, context.CancellationToken);
    }
}
