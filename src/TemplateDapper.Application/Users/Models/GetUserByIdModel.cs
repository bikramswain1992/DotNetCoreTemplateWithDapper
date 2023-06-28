using MediatR;
using TemplateDapper.Application.Common.Responses;
using TemplateDapper.Domain.Dtos;

namespace TemplateDapper.Application.Users.Models;

public sealed record GetUserByIdModel(Guid Id) : IRequest<CommandResponse<UserDto?>>;
