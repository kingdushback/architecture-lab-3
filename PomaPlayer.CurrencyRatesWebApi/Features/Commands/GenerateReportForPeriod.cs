using MediatR;
using Microsoft.AspNetCore.Mvc;
using PomaPlayer.CurrencyRates.Logic.Interfaces.Services;
using PomaPlayer.CurrencyRates.Storage.Database;
using PomaPlayer.CurrencyRates.WebApi.Features.DtoModels;
using System.ComponentModel.DataAnnotations;

namespace PomaPlayer.CurrencyRates.WebApi.Features.Commands;

/// <summary>
/// Запросить отчет за период
/// </summary>
public sealed class GenerateReportForPeriodCommand : IRequest
{
    /// <summary>
    /// Данные для формирования отчета
    /// </summary>
    [Required]
    [FromBody]
    public SaveReportsRequestDto Report { get; init; }
}

public sealed class GenerateReportForPeriodCommandHandler : IRequestHandler<GenerateReportForPeriodCommand>
{
    private readonly DataContext _dataContext;
    private readonly ICronReportService _cronReportService;

    public GenerateReportForPeriodCommandHandler(DataContext dataContext, ICronReportService cronReportService)
    {
        _dataContext = dataContext;
        _cronReportService = cronReportService;
    }

    public async Task Handle(GenerateReportForPeriodCommand request, CancellationToken cancellationToken)
    {
        await _cronReportService.SaveReportsAsync(_dataContext, request.Report.StartDate, request.Report.EndDate, cancellationToken);
        await _dataContext.SaveChangesAsync(cancellationToken);
        return;
    }
}