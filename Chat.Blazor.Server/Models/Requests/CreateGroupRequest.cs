namespace Chat.Blazor.Server.Models.Requests
{
    public class CreateGroupRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string AdminEmail { get; set; }
        public IEnumerable<string> Emails { get; set; }
    }
}
