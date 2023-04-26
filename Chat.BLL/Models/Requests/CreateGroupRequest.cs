using Chat.BLL.DTO;

namespace Chat.BLL.Models.Requests
{
    public class CreateGroupRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string AdminId { get; set; }
        public IEnumerable<UserShortInfoDTO> Members { get; set; }
    }
}
