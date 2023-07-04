namespace Chat.DAL.UoW.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        T GetRepository<T>();

        Task SaveAsync();
    }
}
