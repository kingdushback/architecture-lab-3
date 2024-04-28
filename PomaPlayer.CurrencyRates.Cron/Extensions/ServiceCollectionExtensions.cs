using Microsoft.Extensions.DependencyInjection;
using PomaPlayer.CurrencyRates.Cron.Interfaces.Services;
using PomaPlayer.CurrencyRates.Cron.Refit;
using PomaPlayer.CurrencyRates.Cron.Services;
using Refit;

namespace PomaPlayer.CurrencyRates.Cron.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCronServices(this IServiceCollection services)
    {
        services.AddCronRefitServices();
        services.AddSingleton<ICronService, CronService>();
    }

    private static void AddCronRefitServices(this IServiceCollection services)
    {
        services
            .AddRefitClient<IApiCronService>(_ => new RefitSettings()
            {
                CollectionFormat = CollectionFormat.Multi
            })
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri("https://www.cnb.cz");
            });
    }
}
