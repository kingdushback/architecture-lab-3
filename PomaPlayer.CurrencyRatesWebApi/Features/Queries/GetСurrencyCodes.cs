using MediatR;
using Microsoft.EntityFrameworkCore;
using PomaPlayer.CurrencyRates.Storage.Database;

namespace PomaPlayer.CurrencyRates.WebApi.Features.Queries;

/// <summary>
/// Получить коды валют
/// </summary>
public sealed class GetСurrencyCodesQuery : IRequest<string[]>
{

}

public sealed class GetСurrencyCodesQueryHandler : IRequestHandler<GetСurrencyCodesQuery, string[]>
{
    private readonly DataContext _dataContext;

    public GetСurrencyCodesQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<string[]> Handle(GetСurrencyCodesQuery request, CancellationToken cancellationToken)
    {
        var codes = _dataContext.Reports
            .AsNoTracking()
            .GroupBy(x => x.Code)
            .Select(x => x.Key)
            .OrderBy(x => x)
            .ToArray();

        return codes;
    }
}
