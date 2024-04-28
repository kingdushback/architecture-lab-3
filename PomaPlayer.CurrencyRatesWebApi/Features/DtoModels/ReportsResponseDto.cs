namespace PomaPlayer.CurrencyRates.WebApi.Features.DtoModels;

/// <summary>
/// Отчет о курсе валюты за период
/// </summary>
public sealed record ReportsResponseDto
{
    /// <summary>
    /// Код валюты
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Среднее значение курса
    /// </summary>
    public decimal Avg { get; set; }

    /// <summary>
    /// Максимальный курс
    /// </summary>
    public decimal Max { get; set; }

    /// <summary>
    /// Минимальный курс
    /// </summary>
    public decimal Min { get; set; }
}
