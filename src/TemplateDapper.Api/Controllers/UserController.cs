using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TemplateDapper.Api.Extensions;
using responseCode = TemplateDapper.Application.Common.Responses;
using TemplateDapper.Application.Users.Models;
using TemplateDapper.Domain.Dtos;
using TemplateDapper.Application.Common.Responses;

namespace TemplateDapper.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreateUserModel> _createUserValidator;
    private readonly IValidator<UpdateUserModel> _updateUserValidator;

    public UserController(
        IMediator mediator,
        IValidator<CreateUserModel> createUserValidator,
        IValidator<UpdateUserModel> updateUserValidator)
    {
        _mediator = mediator;
        _createUserValidator = createUserValidator;
        _updateUserValidator = updateUserValidator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetAllUsersModel(), cancellationToken);

        return await this.CreateResponseAsync(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        if(id == default)
        {
            return await this.CreateErrorResponseAsync<UserDto?>("Provide valid user id", responseCode.StatusCode.ModelValidation);
        }

        var response = await _mediator.Send(new GetUserByIdModel(id), cancellationToken);

        return await this.CreateResponseAsync(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserDto), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserModel userModel, CancellationToken cancellationToken)
    {
        var validationResult = await _createUserValidator.ValidateAsync(userModel);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();

            return await this.CreateErrorResponseAsync<UserDto>(errors, responseCode.StatusCode.ModelValidation);
        }

        var response = await _mediator.Send(userModel, cancellationToken);

        return await this.CreateResponseAsync(response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<IActionResult> GetAllUsers(Guid id, [FromBody] UpdateUserModel userModel, CancellationToken cancellationToken)
    {
        if (id == default || id != userModel.Id)
        {
            return await this.CreateErrorResponseAsync<UserDto?>("Provide a valid user id.", responseCode.StatusCode.ModelValidation);
        }
        var validationResult = await _updateUserValidator.ValidateAsync(userModel);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();

            return await this.CreateErrorResponseAsync<UserDto?>(errors, responseCode.StatusCode.ModelValidation);
        }

        var response = await _mediator.Send(userModel, cancellationToken);

        return await this.CreateResponseAsync(response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        if (id == default)
        {
            return await this.CreateErrorResponseAsync<UserDto?>("Provide valid user id", responseCode.StatusCode.ModelValidation);
        }

        var response = await _mediator.Send(new DeleteUserModel(id), cancellationToken);

        return await this.CreateResponseAsync(response);
    }
}
