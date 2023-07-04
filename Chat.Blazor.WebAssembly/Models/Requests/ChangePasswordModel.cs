namespace Chat.Blazor.WebAssembly.Models.Requests
{
    public class ChangePasswordModel
    {
        public string Email { get; set; } = null!;
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public string NewPasswordConfirm { get; set; } = null!;
    }
}