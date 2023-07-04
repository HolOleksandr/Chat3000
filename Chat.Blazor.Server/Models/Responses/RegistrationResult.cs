namespace Chat.Blazor.Server.Models.Responses
{
    public class RegistrationResult
    {
        public bool Success { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
