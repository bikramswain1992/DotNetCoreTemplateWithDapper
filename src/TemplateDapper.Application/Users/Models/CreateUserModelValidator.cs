using FluentValidation;
using System.Text.RegularExpressions;

namespace TemplateDapper.Application.Users.Models;

public class CreateUserModelValidator : AbstractValidator<CreateUserModel>
{
    public CreateUserModelValidator()
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

        RuleFor(c => c.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Must(c =>
            {
                Regex regex = new Regex("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[-+_!@#$%^&*?])");
                bool isValid = regex.IsMatch(c);
                return isValid;
            })
            .Configure(o => o.MessageBuilder = _ => "Password should contain atleast one lower case alphabet, one upper case  alphabet, one number, one special character (-+_!@#$%^&*?) and should be atleast 8 characters long.");
    }
}
