using Chat.BLL.DTO;
using FluentValidation;

namespace Chat.BLL.Validators
{
    public class MessageValidator : AbstractValidator<MessageDTO>
    {
        public MessageValidator()
        {

        }
    }
}
