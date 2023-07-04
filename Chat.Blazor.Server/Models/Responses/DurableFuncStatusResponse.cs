namespace Chat.Blazor.Server.Models.Responses
{
    public class DurableFuncStatusResponse
    {
        public string? Name { get; set; }
        public string? InstanceId { get; set; }
        public string? RuntimeStatus { get; set; }
        public string? CustomStatus { get; set; }
        public FileConvertorResponse? Output { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? LastUpdatedTime { get; set; }
    }
}
