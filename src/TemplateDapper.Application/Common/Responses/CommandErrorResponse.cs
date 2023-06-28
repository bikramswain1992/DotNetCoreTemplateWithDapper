namespace TemplateDapper.Application.Common.Responses;

public sealed class CommandErrorResponse
{
    public List<string> Errors { get; set; } = default!;
}
