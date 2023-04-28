using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DAL.Entities
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Nickname { get; set; }
        public string? Avatar {get; set;}

        //public IList<UserGroup> UserGroups { get; set; }
        public IEnumerable<Group> Groups { get; set; }
        public IEnumerable<GroupInfoView> GroupsInfo { get; set; }
        public IEnumerable<Group> AdminInGroups { get; set; }
    }
}
