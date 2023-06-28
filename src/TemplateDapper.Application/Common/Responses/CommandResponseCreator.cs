namespace TemplateDapper.Application.Common.Responses;

public sealed class CommandResponseCreator<T>
{
    public CommandResponse<T> CreateResponse(T? result, StatusCode statusCode, string? errorMessage = null)
    {
        return new CommandResponse<T>
        {
            Response = result ?? default!,
            StatusCode = statusCode,
            Error = !string.IsNullOrEmpty(errorMessage) ? new CommandErrorResponse
            {
                Errors = new List<string> { errorMessage }
            }
            :
            default!
        };
    }
}
