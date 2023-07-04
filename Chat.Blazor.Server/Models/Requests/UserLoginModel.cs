namespace Chat.Blazor.Server.Models.Requests
{
    public class UserLoginModel
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
