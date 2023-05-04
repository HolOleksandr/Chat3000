using Chat.Blazor.Server.Models.DTO;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Chat.Blazor.Server.Validators
{
    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        public UserDTOValidator()
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

            RuleFor(x => x.Email)
                .NotNull().WithMessage("{PropertyName} is required.")
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .EmailAddress().WithMessage("Please enter your e-mail");

            RuleFor(x => x.Nickname)
                   .Length(2, 25).WithMessage("{PropertyName} must be between 2-25 characters.")
                   .Must(BeValidNickName).WithMessage("Invalid {PropertyName}");

            When(x => x.BirthDate != null, () => {
                RuleFor(x => x.BirthDate)
                    .Must(BeValidMinAge).WithMessage("Your age must be more than 3")
                    .Must(BeValidMaxAge).WithMessage("Congrats if your age is right. Contact the admin to receive a gift.");
            });

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .Matches(new Regex(@"^\+?3?8?(0[5-9][0-9]\d{7})$")).WithMessage("{PropertyName} is not valid");
        }

        private static bool BeValidNickName(string nickName)
        {
            string pattern = "^[a-zA-Z0-9_=\\/]+$";
            return Regex.IsMatch(nickName, pattern);
        }

        private static bool IsValidName(string name)
        {
            if (name == null)
                return false;
            return name.All(Char.IsLetter);
        }

        private static bool BeValidMinAge(DateTime? date)
        {
            if (date.HasValue)
            {
                int minAge = 3;
                var today = DateTime.Now;
                var minBdDate = today.AddYears(-minAge);
                if (minBdDate > date)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        private static bool BeValidMaxAge(DateTime? date)
        {
            if (date.HasValue)
            {
                int maxAge = 60;
                var today = DateTime.Now;
                var manBdDate = today.AddYears(-maxAge);
                if (manBdDate < date)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    
    }
}
