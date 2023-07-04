namespace Chat.DAL.Entities
{
    public class PdfContract
    {
        public Guid? Id { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public string UploaderId { get; set; } = null!;
        public User? Uploader { get; set; }
        public string? ReceiverId { get; set; }
        public User? Receiver { get; set; }
        public bool IsSigned { get; set; }
        public int SignFieldsNum { get; set; }
        public DateTime UploadDate { get; set; }
        public DateTime? SignDate { get; set; }
        public bool IsSyncF { get; set; }
        public bool IsPsPdf { get; set; }
    }
}
