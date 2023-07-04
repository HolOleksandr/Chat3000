using System.ComponentModel.DataAnnotations;

namespace Chat.Blazor.WebAssembly.Models.Requests
{
    public class UserRegistrationModel
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
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