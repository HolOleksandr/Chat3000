using DAL.Repositories.Interfaces;

namespace DAL.UoW.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UsersRepository { get; }
        IMessageRepository MessagesRepository { get; }

        Task SaveAsync();
    }
}
