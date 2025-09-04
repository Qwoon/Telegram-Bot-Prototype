using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace TelegramBotPrototype.Core;

public static class Problems
{
    public static ProblemDetails BadRequest(string? message = null) => new()
    {
        Status = (int)HttpStatusCode.BadRequest,
        Detail = message
    };

    public static ProblemDetails Unauthorized() => new()
    {
        Status = (int)HttpStatusCode.Unauthorized,
    };

    public static ProblemDetails Forbidden(string? message = null) => new()
    {
        Status = (int)HttpStatusCode.Forbidden,
        Detail = message ?? "The requested resource is forbidden."
    };

    public static ProblemDetails NotFound() => new()
    {
        Status = (int)HttpStatusCode.NotFound,
        Detail = "The requested resource could not be found.",
    };

    public static ProblemDetails InvalidInclude(string value) => new()
    {
        Status = (int)HttpStatusCode.BadRequest,
        Detail = $"The requested resource does not have '{value}' relation.",
    };
}

