using Chat.Blazor.Server.Models;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Chat.Blazor.Server.Validators
{
    public class UserRegistrationValidator : AbstractValidator<UserRegistrationModel>
    {
        public UserRegistrationValidator()
        {
            RuleFor(x => x.FirstName)
                 .NotNull().WithMessage("{PropertyName} is required.")
                 .NotEmpty().WithMessage("{PropertyName} must not be empty!!!!.")
                 .Length(2, 25).WithMessage("{PropertyName} must be between 2-25 characters.")
                 .Must(IsValidName).WithMessage("{PropertyName} should be all letters.");

            RuleFor(x => x.LastName)
                .NotNull().WithMessage("{PropertyName} is required.")
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .Length(2, 25).WithMessage("{PropertyName} must be between 2-25 characters.")
                .Must(IsValidName).WithMessage("{PropertyName} should be all letters.");

            RuleFor(x => x.Password)
                .NotNull().WithMessage("{PropertyName} is required.")
                .NotEmpty().WithMessage("{PropertyName} must not be empty.");

            RuleFor(x => x.ConfirmPassword)
                .NotNull().WithMessage("{PropertyName} is required.")
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .Equal(x => x.Password).WithMessage("Passwords do not match");

            RuleFor(x => x.Email)
                .NotNull().WithMessage("{PropertyName} is required.")
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .EmailAddress().WithMessage("Please enter your e-mail");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .Matches(new Regex(@"^\+?3?8?(0[5-9][0-9]\d{7})$")).WithMessage("{PropertyName} is not valid");
        }

        private static bool IsValidName(string name)
        {
            return name.All(Char.IsLetter);
        }
    }
}
