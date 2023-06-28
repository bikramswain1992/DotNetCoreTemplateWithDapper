using FluentValidation;
using System.Text.RegularExpressions;

namespace TemplateDapper.Application.Users.Models;

public class UpdateUserModelValidator : AbstractValidator<UpdateUserModel>
{
    public UpdateUserModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

        RuleFor(c => c.Email)
            .EmailAddress()
            .WithMessage("Provide a valid email.");

        RuleFor(c => c.Role)
            .NotEmpty()
            .WithMessage("Role is required.");
    }
}
