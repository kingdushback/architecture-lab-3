using AutoMapper;
using PomaPlayer.CurrencyRates.Cron.DtoModels;
using PomaPlayer.CurrencyRates.WebApi.Features.DtoModels;

namespace PomaPlayer.CurrencyRates.WebApi.Features.Mappers;

public sealed class ReportsResponseMapper : Profile
{
    public ReportsResponseMapper()
    {
        CreateMap<ReportDto, ReportsResponseDto>();
        CreateMap<ReportsResponseDto, ReportDto>();
    }
}
