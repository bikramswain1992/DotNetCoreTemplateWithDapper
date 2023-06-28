using Mapster;
using MediatR;
using TemplateDapper.Application.Common.Constants;
using TemplateDapper.Application.Common.Responses;
using TemplateDapper.Application.Interfaces;
using TemplateDapper.Application.Users.Models;
using TemplateDapper.Domain.Dtos;
using TemplateDapper.Domain.Entities;
using TemplateDapper.Domain.Requests;

namespace TemplateDapper.Application.Users.Commands;

public class CreateUserCommand : IRequestHandler<CreateUserModel, CommandResponse<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeService _dateTimeService;
    private readonly ICurrentUserService _currentUserService;

    public CreateUserCommand(
        IUnitOfWork unitOfWork,
        IDateTimeService dateTimeService,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _dateTimeService = dateTimeService;
        _currentUserService = currentUserService;
    }

    public async Task<CommandResponse<UserDto>> Handle(CreateUserModel request, CancellationToken cancellationToken)
    {
        var duplicateUser = _unitOfWork
            .UserRepository
            .GetAsync(DatabaseQueries.GetUserByEmailQuery, cancellationToken, new EmailRequest { Email = request.Email });
        if(duplicateUser is not null)
        {
            return new CommandResponseCreator<UserDto>()
                .CreateResponse(default!, StatusCode.BusinessLogicValidation, $"User with email {request.Email} already exists.");
        }

        var userRequest = request.Adapt<User>();
        userRequest.Id = Guid.NewGuid();
        userRequest.CreatedBy = _currentUserService.CurrentUser.Id;
        userRequest.ModifiedBy = _currentUserService.CurrentUser.Id;
        userRequest.CreatedOn = _dateTimeService.Now;
        userRequest.ModifiedOn = _dateTimeService.Now;

        // Hash the password before storing in DB. That step has been left out for the purpose of this template.

        var response = await _unitOfWork.UserRepository.CreateAsync(userRequest, cancellationToken);
        if (response == -1)
        {
            return new CommandResponseCreator<UserDto>()
                .CreateResponse(default!, StatusCode.Unhandled, $"Could not create User with email {request.Email}.");
        }

        var userResponse = userRequest.Adapt<UserDto>();
        return new CommandResponseCreator<UserDto>().CreateResponse(userResponse, StatusCode.Created);
    }
}
