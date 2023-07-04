using Chat.DAL.Entities;

namespace Chat.DAL.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<IEnumerable<User>> GetAllUsersWithFilterAsync(string? search);
        Task<User?> GetUserByStringIdAsync(string id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> IsEmailExistsAsync(string email);
        Task<IEnumerable<string>> GetEmailsListExceptMakerAsync(string makerEmail);
        Task<IEnumerable<User>> GetAllUsersExceptMakerAsync(string? makerEmail);
    }
}
