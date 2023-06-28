using MediatR;
using TemplateDapper.Application.Common.Responses;
using TemplateDapper.Application.Interfaces;
using TemplateDapper.Application.Users.Models;

namespace TemplateDapper.Application.Users.Commands;

public class DeleteUserCommand : IRequestHandler<DeleteUserModel, CommandResponse<Guid?>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommand(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CommandResponse<Guid?>> Handle(DeleteUserModel request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(request.Id, cancellationToken);
        if(user is null)
        {
            return new CommandResponseCreator<Guid?>()
                .CreateResponse(null, StatusCode.NotFound, $"Could not find user with id {request.Id}.");
        }

        var response = await _unitOfWork.UserRepository.DeleteAsync(user.Id, cancellationToken);
        if(response == -1)
        {
            return new CommandResponseCreator<Guid?>()
                .CreateResponse(null, StatusCode.Unhandled, $"Could not find user with id {request.Id}.");
        }

        return new CommandResponseCreator<Guid?>().CreateResponse(request.Id, StatusCode.Success);
    }
}
