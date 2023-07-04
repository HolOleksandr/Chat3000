using Microsoft.AspNetCore.Identity;

namespace Chat.DAL.Entities
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Nickname { get; set; }
        public string? Avatar {get; set;}
        public IEnumerable<Group> Groups { get; set; }
        public IEnumerable<GroupInfoView> GroupsInfo { get; set; }
        public IEnumerable<Group> AdminInGroups { get; set; }
        public IEnumerable<PdfContract>? UploadedPdfContracts { get; set; }
        public IEnumerable<PdfContract>? ReceivedPdfContracts { get; set; }
    }
}
