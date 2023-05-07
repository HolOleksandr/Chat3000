
using Chat.DAL.Data;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Extensions;
using Chat.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DAL.Repositories.Implementation
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ChatDbContext _dbContext;

        public UserRepository(ChatDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetAllUsersWithFilterAsync(string? search)
        {
            var users = await Task.FromResult(
                _dbContext.Users.AsQueryable()
                .Search(search)
                .AsNoTracking());

            return users.AsEnumerable();
        }

        public async Task<User?> GetUserByStringIdAsync(string id)
        {
            var user = await _dbContext.Users.AsQueryable().FirstOrDefaultAsync(x => string.Equals(x.Id, id));
            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = await _dbContext.Users.Include(u => u.Groups).AsQueryable().FirstOrDefaultAsync(x => string.Equals(x.Email, email));
            return user;
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            var user = await GetUserByEmailAsync(email);
            
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public async Task<IEnumerable<string>> GetEmailsListExceptMakerAsync(string makerEmail)
        {
            var emails = await Task.FromResult(_dbContext.Users
                .Where(u => u.Email != makerEmail)
                .Select(u => u.Email)
                .AsNoTracking());
            return emails.AsEnumerable();
        }

        public async Task<IEnumerable<User>> GetAllUsersExceptMakerAsync(string? makerEmail)
        {
            var users = await Task.FromResult(_dbContext.Users
                .Where(u => u.Email != makerEmail)
                .AsNoTracking());

            return users.AsEnumerable();
        }


    }
}
