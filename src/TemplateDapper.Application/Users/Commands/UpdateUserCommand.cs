using Mapster;
using MediatR;
using TemplateDapper.Application.Common.Responses;
using TemplateDapper.Application.Interfaces;
using TemplateDapper.Application.Users.Models;
using TemplateDapper.Domain.Dtos;

namespace TemplateDapper.Application.Users.Commands;

public class UpdateUserCommand : IRequestHandler<UpdateUserModel, CommandResponse<UserDto?>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeService _dateTimeService;
    private readonly ICurrentUserService _currentUserService;

    public UpdateUserCommand(
        IUnitOfWork unitOfWork,
        IDateTimeService dateTimeService,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _dateTimeService = dateTimeService;
        _currentUserService = currentUserService;
    }

    public async Task<CommandResponse<UserDto?>> Handle(UpdateUserModel request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.UserRepository.GetByIdAsync(request.Id, cancellationToken);
        if (existingUser is null)
        {
            return new CommandResponseCreator<UserDto?>().CreateResponse(default!, StatusCode.NotFound, $"Could not find user with id {request.Id}.");
        }

        request.Adapt(existingUser);
        existingUser.ModifiedBy = _currentUserService.CurrentUser.Id;
        existingUser.ModifiedOn = _dateTimeService.Now;

        var response = await _unitOfWork.UserRepository.UpdateAsync(existingUser, cancellationToken);
        if(response == -1)
        {
            return new CommandResponseCreator<UserDto?>().CreateResponse(default!, StatusCode.Unhandled, $"Could not update user with id {request.Id}.");
        }

        var userResponse = existingUser.Adapt<UserDto>();
        return new CommandResponseCreator<UserDto?>().CreateResponse(userResponse, StatusCode.Success);
    }
}
