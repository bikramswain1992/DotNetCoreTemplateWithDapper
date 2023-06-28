namespace TemplateDapper.Application.Common.Responses;

public class ErrorResponse
{
    public StatusCode StatusCode { get; set; }
    public List<string> Errors { get; set; } = default!;
}
