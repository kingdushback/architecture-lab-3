using System.Net;

namespace PomaPlayer.CurrencyRates.Logic.Exceptions;

public class CurRateException : Exception
{
    public string ErrorCode { get; set; }

    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.UnprocessableEntity;

    public object ErrorDetails { get; set; }
}
