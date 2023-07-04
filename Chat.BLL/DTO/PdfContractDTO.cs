namespace Chat.BLL.DTO
{
    public class PdfContractDTO
    {
        public Guid Id { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public string UploaderId { get; set; } = null!;
        public UserDTO? Uploader { get; set; }
        public string? ReceiverId { get; set; } = null!;
        public UserDTO? Receiver { get; set; }
        public bool IsSigned { get; set; }
        public int SignFieldsNum { get; set; } = 0;
        public DateTime? UploadDate { get; set; }
        public DateTime? SignDate { get; set; }
        public bool IsSyncF { get; set; }
        public bool IsPsPdf { get; set; }
    }
}
