using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PomaPlayer.CurrencyRates.Logic.Extensions;
using PomaPlayer.CurrencyRates.Quartz.Jobs;
using PomaPlayer.CurrencyRates.Storage.Database;
using Quartz;
using Quartz.AspNetCore;
using System.Reflection;

namespace PomaPlayer.CurrencyRates.Quartz.Extensioncs;

public static class ServiceCollectionExtensions
{
    private const int MaxQuartzConcurrency = 10;

    public static void AddQuartzServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataContext(configuration);

        services.AddLogicServices();
        services.RegisterQuartzServices(configuration);
        services.AddSwaggerService();
    }

    private static void RegisterQuartzServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartzServer(opt =>
        {
            opt.AwaitApplicationStarted = true;
            opt.WaitForJobsToComplete = false;
        });

        services.AddQuartz(q =>
        {
            q.SchedulerId = "AUTO";

            q.UseMicrosoftDependencyInjectionJobFactory();
            q.UseDefaultThreadPool(MaxQuartzConcurrency);
            q.MaxBatchSize = MaxQuartzConcurrency;

            q.UsePersistentStore(x =>
            {
                x.UseClustering();
                x.UseSqlServer(configuration.GetDefaultConnectionString());
                x.UseNewtonsoftJsonSerializer();
            });

            q.RegisterJobs(configuration);
        });
    }

    private static void RegisterJobs(this IServiceCollectionQuartzConfigurator services, IConfiguration configuration)
    {
        services.ScheduleJob<ReportDailyJob>(
            trigger =>
            {
                trigger.WithIdentity(ReportDailyJob.JobName);
                trigger.WithCronSchedule(
                    configuration.GetReportDailyCronSchedule(),
                    schedule => schedule.WithMisfireHandlingInstructionIgnoreMisfires());
            },
            job =>
            {
                job.DisallowConcurrentExecution();
                job.WithIdentity(ReportDailyJob.JobName);
                job.RequestRecovery();
                job.StoreDurably();
            });
    }

    private static void AddDataContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
        options.UseSqlServer(configuration.GetDefaultConnectionString(), o =>
        {
            o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        }));
    }

    private static void AddSwaggerService(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Quartz", Version = "v1" });
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            foreach (FileInfo file in new DirectoryInfo(AppContext.BaseDirectory).GetFiles(
                         Assembly.GetExecutingAssembly().GetName().Name! + ".xml"))
                c.IncludeXmlComments(file.FullName);

            c.EnableAnnotations(enableAnnotationsForInheritance: true,
                enableAnnotationsForPolymorphism: true
            );
        });
    }

}
