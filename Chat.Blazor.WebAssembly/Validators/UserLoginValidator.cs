using Chat.Blazor.WebAssembly.Models.Requests;
using FluentValidation;

namespace Chat.Blazor.WebAssembly.Validators
{
    public class UserLoginValidator : AbstractValidator<UserLoginModel>
    {
        public UserLoginValidator() 
        {
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("E-mail is required (Login)")
                .EmailAddress().WithMessage("Please enter your e-mail");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
