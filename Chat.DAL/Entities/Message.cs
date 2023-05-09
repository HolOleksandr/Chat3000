using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Chat.DAL.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; } = null!;
        public User? Sender { get; set; }
        public string? ReceiverId { get; set; }
        public User? Receiver { get; set; }
        public int GroupId { get; set; }
        public Group? Group { get; set; }
        public string? Text { get; set; }
        public DateTime SendDate { get; set; }
    }
}
