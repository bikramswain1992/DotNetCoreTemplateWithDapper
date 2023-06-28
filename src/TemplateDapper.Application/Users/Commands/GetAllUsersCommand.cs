using Mapster;
using MediatR;
using TemplateDapper.Application.Common.Responses;
using TemplateDapper.Application.Interfaces;
using TemplateDapper.Application.Users.Models;
using TemplateDapper.Domain.Dtos;

namespace TemplateDapper.Application.Users.Commands;

public class GetAllUsersCommand : IRequestHandler<GetAllUsersModel, CommandResponse<IEnumerable<UserDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllUsersCommand(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CommandResponse<IEnumerable<UserDto>>> Handle(GetAllUsersModel request, CancellationToken cancellationToken)
    {
        var users = await _unitOfWork.UserRepository.GetAllAsync(cancellationToken);

        if(!users.Any())
        {
            return new CommandResponseCreator<IEnumerable<UserDto>>()
                .CreateResponse(default!, StatusCode.NotFound, "Could not find any users.");
        }

        var userResponse = users.Adapt<IEnumerable<UserDto>>();

        return new CommandResponseCreator<IEnumerable<UserDto>>()
            .CreateResponse(userResponse, StatusCode.Success);
    }
}
