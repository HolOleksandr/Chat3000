namespace Chat.DAL.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string AdminId { get; set; } = null!;
        public User Admin { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public IList<User> Users { get; set; } = null!;
        public ICollection<Message>? Messages { get; set; }
        public GroupInfoView GroupInfo { get; set; }
    }
}
