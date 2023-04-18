using Chat.BLL.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.BLL.Validators
{
    public class UserLoginValidator : AbstractValidator<UserLoginModel>
    {
        public UserLoginValidator()
        {
            RuleFor(u => u.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Please enter your e-mail");

            RuleFor(u => u.Password).NotEmpty();
        }
    }
}
