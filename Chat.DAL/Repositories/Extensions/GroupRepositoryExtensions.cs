using Chat.DAL.Entities;

namespace Chat.DAL.Repositories.Extensions
{
    public static class GroupRepositoryExtensions
    {
        public static IQueryable<Group> SearchInGroups(this IQueryable<Group> groups, string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return groups;

            var lowerCaseSearchText = searchText.Trim().ToLower();
            var searchResult = groups.Where(p =>
                p.Id.ToString().ToLower().Contains(lowerCaseSearchText) ||
                p.Name.ToLower().Contains(lowerCaseSearchText) ||
                p.CreationDate.ToString().ToLower().Contains(lowerCaseSearchText) ||
                p.Admin.Email.ToLower().Contains(lowerCaseSearchText) ||
                p.Description.ToLower().Contains(lowerCaseSearchText));

            return searchResult;
        }
    }
}
