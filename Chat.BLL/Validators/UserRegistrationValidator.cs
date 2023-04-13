using Chat.DAL.Entities.AuthModels;

using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chat.BLL.Validators
{
    public class UserRegistrationValidator : AbstractValidator<UserRegistrationModel>
    {
        public UserRegistrationValidator()
        {
            RuleFor(x => x.FirstName)
                .NotNull()
                .NotEmpty().WithMessage("{PropertyName} should be not empty.")
                .Length(2, 25)
                .Must(IsValidName).WithMessage("{PropertyName} should be all letters.");

            RuleFor(x => x.LastName)
                .NotNull()
                .NotEmpty().WithMessage("{PropertyName} should be not empty.")
                .Length(2, 25)
                .Must(IsValidName).WithMessage("{PropertyName} should be all letters.");


            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Please enter your e-mail");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .NotNull().WithMessage("{PropertyName} is required.")
                .Matches(new Regex(@"^\+?3?8?(0[5-9][0-9]\d{7})$")).WithMessage("{PropertyName} is not valid");

        }

        private static bool IsValidName(string name)
        {
            return name.All(Char.IsLetter);
        }
    }
}
