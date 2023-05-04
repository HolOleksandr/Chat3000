using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DAL.Entities
{
    public class GroupInfoView
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string AdminId { get; set; } = null!;
        public string AdminEmail { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public int TotalMessages { get; set; }
        public int Members { get; set; }
        public IList<User> Users { get; set; } = null!;

        public Group Group { get; set; }
    }
}
