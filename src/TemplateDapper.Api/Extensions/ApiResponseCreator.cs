using Microsoft.AspNetCore.Mvc;
using TemplateDapper.Application.Common.Responses;

namespace TemplateDapper.Api.Extensions;

public static class ApiResponseCreator
{
    public static async Task<IActionResult> CreateResponseAsync<T>(this ControllerBase controller, CommandResponse<T> response)
    {
        return await Task.FromResult(response.StatusCode switch
        {
            StatusCode.Success => controller.Ok(response.Response),
            StatusCode.Created => controller.Created(controller.Url.ToString()!, response.Response),
            StatusCode.Forbidden => controller.StatusCode(StatusCodes.Status403Forbidden, GetErrorResponse(response)),
            StatusCode.NotFound => controller.StatusCode(StatusCodes.Status404NotFound, GetErrorResponse(response)),
            StatusCode.Unhandled => controller.StatusCode(StatusCodes.Status500InternalServerError, GetErrorResponse(response)),
            _ => controller.BadRequest(GetErrorResponse(response))
        });
    }

    public static async Task<IActionResult> CreateErrorResponseAsync<T>(this ControllerBase controller, IEnumerable<string> errors, StatusCode statusCode)
    {
        return await controller.CreateResponseAsync<T>(
            new CommandResponse<T>
            {
                StatusCode = statusCode,
                Error = new CommandErrorResponse
                {
                    Errors = errors.ToList()
                }
            });
    }

    public static async Task<IActionResult> CreateErrorResponseAsync<T>(this ControllerBase controller, string error, StatusCode statusCode)
    {
        return await controller.CreateResponseAsync<T>(
            new CommandResponse<T>
            {
                StatusCode = statusCode,
                Error = new CommandErrorResponse
                {
                    Errors = new List<string> { error }
                }
            });
    }

    private static ErrorResponse GetErrorResponse<T>(CommandResponse<T> response)
    {
        return new ErrorResponse
        {
            StatusCode = response.StatusCode,
            Errors = response.Error.Errors
        };
    }
}
