using MediatR;
using Microsoft.AspNetCore.Mvc;
using PomaPlayer.CurrencyRates.Logic.Exceptions;
using PomaPlayer.CurrencyRates.Storage.Database;
using PomaPlayer.CurrencyRates.WebApi.Features.DtoModels;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace PomaPlayer.CurrencyRates.WebApi.Features.Queries;

/// <summary>
/// Получить код валюты за текущее число
/// </summary>
public sealed class GetTodayCurrencyRateQuery : IRequest<CurrencyRate>
{
    /// <summary>
    /// Код валюты
    /// </summary>
    [Required]
    [FromQuery]
    public string Code { get; set; }
}

public sealed class GetTodayCurrencyRateQueryHandler : IRequestHandler<GetTodayCurrencyRateQuery, CurrencyRate>
{
    private readonly DataContext _dataContext;

    public GetTodayCurrencyRateQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<CurrencyRate> Handle(GetTodayCurrencyRateQuery request, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow;
        var date = new DateOnly(today.Year, today.Month, today.Day);

        var rate = _dataContext.Reports.FirstOrDefault(x => x.Code == request.Code && x.Date == date)
            ?? throw new CurRateException { ErrorCode = "INVALID_QUERY", StatusCode = HttpStatusCode.BadRequest, ErrorDetails = $"Данные за текущий день не найдены в базе данных или задан неверный код валюты" };

        var rateDto = new CurrencyRate
        {
            Date = rate.Date,
            Code = rate.Code,
            Value = rate.Rate
        };

        return rateDto;
    }
}
