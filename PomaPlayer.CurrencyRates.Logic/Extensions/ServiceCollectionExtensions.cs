using Microsoft.Extensions.DependencyInjection;
using PomaPlayer.CurrencyRates.Cron.Extensions;
using PomaPlayer.CurrencyRates.Logic.Interfaces.Repositories;
using PomaPlayer.CurrencyRates.Logic.Interfaces.Services;
using PomaPlayer.CurrencyRates.Logic.Repositories;
using PomaPlayer.CurrencyRates.Logic.Services;

namespace PomaPlayer.CurrencyRates.Logic.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddLogicServices(this IServiceCollection services)
    {
        services.AddCronServices();

        services.AddSingleton<IRepository, Repository>();
        services.AddSingleton<ICronReportService, CronReportService>();
    }
}
