using Chat.BLL.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.BLL.Validators
{
    public class MessageValidator : AbstractValidator<MessageDTO>
    {
        public MessageValidator()
        {

        }
    }
}
