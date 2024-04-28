using MediatR;
using Microsoft.AspNetCore.Mvc;
using PomaPlayer.CurrencyRates.WebApi.Features.Commands;
using PomaPlayer.CurrencyRates.WebApi.Features.DtoModels;
using PomaPlayer.CurrencyRates.WebApi.Features.Queries;

namespace PomaPlayer.CurrencyRates.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ManageController : Controller
{
    private readonly IMediator _mediator;

    public ManageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Получить отчет о курсах валют за период
    /// </summary>
    /// <param name="query">Dto параметр</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Отчет о курсах валют за период</returns>
    [HttpPost(nameof(GetReportOnCurrencies), Name = nameof(GetReportOnCurrencies))]
    public async Task<ActionResult<ReportsResponseDto[]>> GetReportOnCurrencies(GetReportOnCurrenciesQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Запросить отчет за период
    /// </summary>
    /// <param name="command">Dto параметр</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    [HttpPost(nameof(GenerateReportForPeriod), Name = nameof(GenerateReportForPeriod))]
    public async Task<ActionResult> GenerateReportForPeriod(GenerateReportForPeriodCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Получить коды валют
    /// </summary>
    /// <param name="query">Dto параметр</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Коды валют</returns>
    [HttpGet(nameof(GetСurrencyCodes), Name = nameof(GetСurrencyCodes))]
    public async Task<ActionResult<string[]>> GetСurrencyCodes(CancellationToken cancellationToken)
    {
        var codes = await _mediator.Send(new GetСurrencyCodesQuery(), cancellationToken);
        return Ok(codes);
    }

    /// <summary>
    /// Получить курс валюты за текущий день
    /// </summary>
    /// <param name="query">Dto параметр</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Курс валюты за текущий день</returns>
    [HttpGet(nameof(GetTodayCurrencyCode), Name = nameof(GetTodayCurrencyCode))]
    public async Task<ActionResult<CurrencyRate>> GetTodayCurrencyCode(GetTodayCurrencyRateQuery query, CancellationToken cancellationToken)
    {
        var rate = await _mediator.Send(query, cancellationToken);
        return Ok(rate);
    }
}
