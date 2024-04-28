using Microsoft.Extensions.Logging;
using Quartz;

namespace PomaPlayer.CurrencyRates.Quartz.Jobs.Base;

public abstract class BaseJob : IJob
{
    private readonly string _jobName;
    protected readonly ILogger _logger;
    protected readonly IServiceProvider _serviceProvider;

    protected BaseJob(
        ILogger logger,
        IServiceProvider serviceProvider)
    {
        _jobName = GetType().Name;

        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var plannedStartTime = context.ScheduledFireTimeUtc;
        var actualFireTime = context.FireTimeUtc;

        JobContext.SetTrigger(context.Trigger);
        try
        {
            var delay = actualFireTime - plannedStartTime;

            // for case when fire time earlier than planned
            if (delay is { TotalMilliseconds: < 0 })
            {
                delay = TimeSpan.Zero;
            }

            _logger.LogInformation("Job {JobName} is started", _jobName);

            await DoWork(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during execution job {JobName}", _jobName);
        }
        finally
        {
            _logger.LogInformation("Job {JobName} is finished", _jobName);
        }

    }

    protected abstract Task DoWork(IJobExecutionContext context);
}
