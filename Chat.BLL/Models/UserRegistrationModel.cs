using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.BLL.Models
{
    public class UserRegistrationModel : UserLoginModel
    {
        public string ConfirmPassword { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime? BirthDate { get; set; }
        public string? Nickname { get; set; }
        public string? Avatar { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = null!;

    }
}
