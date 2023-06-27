namespace TemplateDapper.Domain.ValueObjects;

public sealed class CurrentUser
{
    public int Id { get; init; }
    public string Email { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Role { get; init; } = default!;
}
