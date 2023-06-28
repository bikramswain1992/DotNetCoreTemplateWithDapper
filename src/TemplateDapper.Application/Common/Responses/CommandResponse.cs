namespace TemplateDapper.Application.Common.Responses;

public sealed class CommandResponse<T>
{
    public T Response { get; set; } = default!;
    public StatusCode StatusCode { get; set; }
    public CommandErrorResponse Error { get; set; } = default!;
}
