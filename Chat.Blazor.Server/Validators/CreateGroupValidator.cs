using Chat.Blazor.Server.Models.DTO;
using Chat.Blazor.Server.Models.Requests;
using FluentValidation;

namespace Chat.Blazor.Server.Validators
{
    public class CreateGroupValidator : AbstractValidator<CreateGroupRequest>
    {
        public CreateGroupValidator()
        {
            RuleFor(x => x.Name)
                 .NotEmpty().WithMessage("Group name is required")
                 .Length(2, 15).WithMessage("Group name must be between 2-10 characters.");

            RuleFor(x => x.Description)
                 .NotEmpty().WithMessage("Description is required")
                 .Length(5, 150).WithMessage("Description must be between 5-150 characters.");
        }
    }
}
