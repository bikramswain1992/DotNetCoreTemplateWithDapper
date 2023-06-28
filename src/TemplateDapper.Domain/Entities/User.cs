namespace TemplateDapper.Domain.Entities;

public class User : AuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Role { get; set; } = default!;
}
