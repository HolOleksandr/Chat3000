using Chat.BLL.Models.AzureFuncModels;

namespace Chat.BLL.Models.Responses
{
    public class DurableFuncStatusResponse
    {
        public string Name { get; set; }
        public string InstanceId { get; set; }
        public string RuntimeStatus { get; set; }
        public FileInfoModel Input { get; set; }
        public FileConvertorResponse Output { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
    }
}
