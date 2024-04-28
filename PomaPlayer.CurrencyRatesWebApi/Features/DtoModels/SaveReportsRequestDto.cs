using System.ComponentModel.DataAnnotations;

namespace PomaPlayer.CurrencyRates.WebApi.Features.DtoModels;

/// <summary>
/// Данные формирования отчета
/// </summary>
public sealed record SaveReportsRequestDto
{
    /// <summary>
    /// Дата начала периода
    /// </summary>
    [Required]
    public DateOnly StartDate { get; init; }

    /// <summary>
    /// Дата конца периода
    /// </summary>
    [Required]
    public DateOnly EndDate { get; init; }
}
