using Mapster;
using MediatR;
using TemplateDapper.Application.Common.Responses;
using TemplateDapper.Application.Interfaces;
using TemplateDapper.Application.Users.Models;
using TemplateDapper.Domain.Dtos;

namespace TemplateDapper.Application.Users.Commands;

public class GetUserByIdCommand : IRequestHandler<GetUserByIdModel, CommandResponse<UserDto?>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserByIdCommand(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CommandResponse<UserDto?>> Handle(GetUserByIdModel request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(request.Id, cancellationToken);

        if(user is null)
        {
            return new CommandResponseCreator<UserDto?>().CreateResponse(default!, StatusCode.NotFound, $"Could not find user with id {request.Id}.");
        }

        var userResponse = user.Adapt<UserDto>();

        return new CommandResponseCreator<UserDto?>().CreateResponse(userResponse, StatusCode.Success);
    }
}
