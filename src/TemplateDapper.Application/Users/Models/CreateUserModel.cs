using MediatR;
using TemplateDapper.Application.Common.Responses;
using TemplateDapper.Domain.Dtos;

namespace TemplateDapper.Application.Users.Models;

public sealed record CreateUserModel : IRequest<CommandResponse<UserDto>>
{
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
    public string Role { get; init; } = default!;
}
