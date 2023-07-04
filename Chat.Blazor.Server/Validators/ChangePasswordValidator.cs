using Chat.Blazor.Server.Models.Requests;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Chat.Blazor.Server.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .EmailAddress().WithMessage("Please enter your e-mail");

            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.");
           
            RuleFor(x => x.NewPasswordConfirm)
             .NotEmpty().WithMessage("Password confirmation is required.")
             .Equal(x => x.NewPassword).WithMessage("New passwords do not match");
        }
    }
}
