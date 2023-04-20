using Chat.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DAL.Repositories.Extensions
{
    public static class UserRepositoryExtensions
    {
        public static IQueryable<User> Search(this IQueryable<User> users, string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return users;

            var lowerCaseSearchText = searchText.Trim().ToLower();
            var searchResult = users.Where(p =>
                p.Id.ToLower().Contains(lowerCaseSearchText) ||
                p.FirstName.ToLower().Contains(lowerCaseSearchText) ||
                p.LastName.ToLower().Contains(lowerCaseSearchText) ||
                p.Nickname.ToLower().Contains(lowerCaseSearchText) ||
                p.Email.ToLower().Contains(lowerCaseSearchText) ||
                p.PhoneNumber.ToLower().Contains(lowerCaseSearchText));
            return searchResult;
        }
    }
}


