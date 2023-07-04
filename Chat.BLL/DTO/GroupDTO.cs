namespace Chat.BLL.DTO
{
    public class GroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AdminId { get; set; }
        public UserDTO Admin { get; set; }
        public DateTime CreationDate { get; set; }
        public IList<UserDTO> Users { get; set; }
        public int UsersCount { get; set; }
    }
}
