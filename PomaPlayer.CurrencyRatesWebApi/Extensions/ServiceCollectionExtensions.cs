using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PomaPlayer.CurrencyRates.Logic.Extensions;
using PomaPlayer.CurrencyRates.Storage.Database;
using PomaPlayer.CurrencyRates.WebApi.Features.Mappers;
using System.Reflection;

namespace PomaPlayer.CurrencyRates.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataContext(configuration);
        services.AddLogicServices();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });

        services.AddWebMappers();

        services.AddSwaggerService();
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
            c.SwaggerDoc("v1", new OpenApiInfo() { Title = "CurrencyRates", Version = "v1" });
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            foreach (FileInfo file in new DirectoryInfo(AppContext.BaseDirectory).GetFiles(
                         Assembly.GetExecutingAssembly().GetName().Name! + ".xml"))
                c.IncludeXmlComments(file.FullName);

            c.EnableAnnotations(enableAnnotationsForInheritance: true,
                enableAnnotationsForPolymorphism: true
            );
        });
    }

    private static void AddWebMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ReportsResponseMapper));
    }
}
