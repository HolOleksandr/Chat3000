using Chat.Blazor.Server.Models;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Chat.Blazor.Server.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotNull().WithMessage("{PropertyName} is required.")
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .EmailAddress().WithMessage("Please enter your e-mail");

            RuleFor(x => x.OldPassword)
                .NotNull().WithMessage("{PropertyName} is required.")
                .NotEmpty().WithMessage("{PropertyName} must not be empty.");

            RuleFor(x => x.NewPassword)
                .NotNull().WithMessage("{PropertyName} is required.")
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .Equal(x => x.OldPassword).WithMessage("Passwords do not match");
        }
    
    }
}
