namespace PomaPlayer.CurrencyRates.WebApi.Features.DtoModels;

/// <summary>
/// Курс валюты
/// </summary>
public sealed record CurrencyRate
{
    /// <summary>
    /// Код валюты
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Дата
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Курс валюты
    /// </summary>
    public decimal Value { get; set; }
}
