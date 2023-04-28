using Chat.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DAL.Repositories.Extensions
{
    public static class GroupInfoViewExtensions
    {
        public static IQueryable<GroupInfoView> SearchInGroupInfo(this IQueryable<GroupInfoView> groups, string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return groups;

            var lowerCaseSearchText = searchText.Trim().ToLower();
            var searchResult = groups.Where(p =>
                p.Id.ToString().ToLower().Contains(lowerCaseSearchText) ||
                p.Name.ToLower().Contains(lowerCaseSearchText) ||
                p.CreationDate.ToString().ToLower().Contains(lowerCaseSearchText) ||
                p.Description.ToLower().Contains(lowerCaseSearchText) ||
                p.AdminEmail.ToLower().Contains(lowerCaseSearchText) ||
                p.TotalMessages.ToString().ToLower().Contains(lowerCaseSearchText) ||
                p.Members.ToString().ToLower().Contains(lowerCaseSearchText));

            return searchResult;
        }
    }
}
