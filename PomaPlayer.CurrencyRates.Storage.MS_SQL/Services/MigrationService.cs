using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PomaPlayer.CurrencyRates.Storage.Database;

namespace PomaPlayer.CurrencyRates.Storage.MS_SQL.Services;

public class MigrationService
{
    private readonly ILogger<MigrationService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public MigrationService(
        ILogger<MigrationService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public bool ApplyMigrations()
    {
        lock (typeof(MigrationService))
        {
            try
            {
                _logger.LogInformation("Применение миграций");

                using var scope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
                using var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

                dbContext.Database.SetCommandTimeout(3600);
                _logger.LogInformation("Подключение: " + dbContext.Database.GetConnectionString());

                var IsExists = dbContext!.GetService<IDatabaseCreator>() is RelationalDatabaseCreator DbCreator && DbCreator.Exists();
                if (!IsExists)
                {
                    var migrations = dbContext.Database.GetPendingMigrations().ToArray();
                    if (migrations.Length == 0)
                        throw new Exception("Migrations not found");

                    dbContext!.Database.Migrate();
                    dbContext!.SaveChanges();
                }

                _logger.LogWarning("База данных обновлена");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Не удалось обновить базу данных");
            }

            return false;
        }
    }
}
