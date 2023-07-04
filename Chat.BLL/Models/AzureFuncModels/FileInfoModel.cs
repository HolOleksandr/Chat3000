namespace Chat.BLL.Models.AzureFuncModels
{
    public class FileInfoModel
    {
        public string UploaderId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
    }
}
