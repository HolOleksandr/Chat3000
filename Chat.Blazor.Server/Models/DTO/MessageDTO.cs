

namespace Chat.Blazor.Server.Models.DTO
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public string SenderId { get; set; } = null!;
        public UserDTO? Sender { get; set; } = null!;
        public string? ReceiverId { get; set; }
        public UserDTO? Receiver { get; set; }
        public int GroupId { get; set; }
        public string? Text { get; set; }
        public DateTime SendDate { get; set; }
    }
}
