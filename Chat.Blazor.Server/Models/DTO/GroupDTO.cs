using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Blazor.Server.Models.DTO
{
    public class GroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AdminId { get; set; }
        public UserDTO Admin { get; set; }
        public DateTime CreationDate { get; set; }
        //public IList<UserGroup> UserGroups { get; set; }
        public IList<UserDTO> Users { get; set; }
        public int UsersCount { get; set; }

    }
}
