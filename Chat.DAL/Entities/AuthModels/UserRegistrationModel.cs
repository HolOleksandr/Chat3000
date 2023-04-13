using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DAL.Entities.AuthModels
{
    public class UserRegistrationModel : UserLoginModel
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = null!;

    }
}
