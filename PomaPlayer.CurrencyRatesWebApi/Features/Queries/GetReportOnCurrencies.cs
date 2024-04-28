using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PomaPlayer.CurrencyRates.Logic.Interfaces.Services;
using PomaPlayer.CurrencyRates.Storage.Database;
using PomaPlayer.CurrencyRates.WebApi.Features.DtoModels;
using System.ComponentModel.DataAnnotations;

namespace PomaPlayer.CurrencyRates.WebApi.Features.Queries;

/// <summary>
/// Получить отчет о курсах валют за период
/// </summary>
public sealed class GetReportOnCurrenciesQuery : IRequest<ReportsResponseDto[]>
{
    /// <summary>
    /// Дата начала периода
    /// </summary>
    [Required]
    [FromQuery]
    public DateOnly StartDate { get; init; }

    /// <summary>
    /// Дата конца периода
    /// </summary>
    [Required]
    [FromQuery]
    public DateOnly EndDate { get; init; }

    /// <summary>
    /// Коды валют
    /// </summary>
    [Required]
    [FromBody]
    public string[] Codes { get; init; }
}

public sealed class GetReportOnCurrenciesQueryHandler : IRequestHandler<GetReportOnCurrenciesQuery, ReportsResponseDto[]>
{
    private readonly IMapper _mapper;
    private readonly DataContext _dataContext;
    private readonly ICronReportService _cronReportService;

    public GetReportOnCurrenciesQueryHandler(
        IMapper mapper,
        DataContext dataContext,
        ICronReportService cronReportService)
    {
        _mapper = mapper;
        _dataContext = dataContext;
        _cronReportService = cronReportService;
    }

    public async Task<ReportsResponseDto[]> Handle(GetReportOnCurrenciesQuery request, CancellationToken cancellationToken)
    {
        var result = await _cronReportService.CalculateReportAsync(_dataContext, request.StartDate, request.EndDate, request.Codes, cancellationToken);
        var reports = _mapper.Map<ReportsResponseDto[]>(result);

        return reports;
    }
}
