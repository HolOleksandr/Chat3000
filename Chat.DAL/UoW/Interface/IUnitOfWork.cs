using Chat.DAL.Repositories.Interfaces;
using Chat.DAL.Repositories.Implementation;

namespace Chat.DAL.UoW.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        T GetRepository<T>();

        Task SaveAsync();
    }

}
