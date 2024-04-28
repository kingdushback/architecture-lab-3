using Refit;

namespace PomaPlayer.CurrencyRates.Cron.Refit;

public interface IApiCronService
{
    [Get("/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt")]
    Task<string> GetDailyReport([Query] string date, CancellationToken cancellationToken = default);
}
