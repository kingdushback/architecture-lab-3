using PomaPlayer.CurrencyRates.Logic.Exceptions;
using PomaPlayer.CurrencyRates.WebApi.Middlewares.DtoModels;
using System.Net;

namespace PomaPlayer.CurrencyRates.WebApi.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException ex)
            when (context.RequestAborted.IsCancellationRequested)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new ErrorResponseDto
            {
                ErrorCode = "CANCELED",
                ErrorMessage = "Request was cancelled"
            });
        }
        catch (Exception exception)
        {
            int httpStatusCode;
            ErrorResponseDto errorResponse;

            var result = Handle(exception);
            if (result != null)
            {
                errorResponse = result.Value.Response;
                httpStatusCode = (int)result.Value.Code;
            }
            else
            {
                httpStatusCode = StatusCodes.Status500InternalServerError;
                errorResponse = new ErrorResponseDto
                {
                    ErrorCode = "INTERNAL_ERROR",
                    ErrorMessage = "Something went wrong. Please try again later."
                };
            }

            context.Response.StatusCode = httpStatusCode;
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }

    private (ErrorResponseDto Response, HttpStatusCode Code)? Handle(Exception ex)
    {
        if (ex is CurRateException exception)
        {
            var errorResponse = new ErrorResponseDto
            {
                ErrorCode = exception.ErrorCode,
                ErrorDetails = exception.ErrorDetails
            };

            return (errorResponse, exception.StatusCode);
        }

        return null;
    }
}
