using System.Text.Json.Serialization;

namespace PomaPlayer.CurrencyRates.Quartz.Middlewares.DtoModels;

public class ErrorResponseDto
{
    public string ErrorCode { get; init; }

    public string ErrorMessage { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object ErrorDetails { get; init; }
}
