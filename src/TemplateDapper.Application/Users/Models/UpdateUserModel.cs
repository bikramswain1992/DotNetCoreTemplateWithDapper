using MediatR;
using TemplateDapper.Application.Common.Responses;
using TemplateDapper.Domain.Dtos;

namespace TemplateDapper.Application.Users.Models;

public sealed record UpdateUserModel : IRequest<CommandResponse<UserDto?>>
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Role { get; init; } = default!;
}
