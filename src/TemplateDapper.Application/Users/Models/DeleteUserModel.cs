using MediatR;
using TemplateDapper.Application.Common.Responses;

namespace TemplateDapper.Application.Users.Models;
public sealed record DeleteUserModel(Guid Id) : IRequest<CommandResponse<Guid?>>;
