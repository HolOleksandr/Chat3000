using Chat.DAL.Entities.AuthModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.BLL.Validators
{
    public class UserChangePassValidator : AbstractValidator<ChangePasswordModel>
    {
        public UserChangePassValidator()
        {
            RuleFor(u => u.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Please enter your e-mail");

            RuleFor(u => u.OldPassword).NotEmpty();

            RuleFor(u => u.NewPassword)
                .NotEmpty()
                .MinimumLength(5);

        }
    }
}
