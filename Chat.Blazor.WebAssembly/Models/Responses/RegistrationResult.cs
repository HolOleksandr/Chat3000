namespace Chat.Blazor.WebAssembly.Models.Responses
{
    public class RegistrationResult
    {
        public bool Success { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}