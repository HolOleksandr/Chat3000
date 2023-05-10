using Chat.DAL.Data;
using Chat.DAL.Repositories.Interfaces;
using Chat.DAL.Repositories.Implementation;
using Chat.DAL.UoW.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.DAL.UoW.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChatDbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDictionary<string, object> _repositories;

        public UnitOfWork(ChatDbContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _repositories = new Dictionary<string, object>();
            _serviceProvider = serviceProvider;
        }

        public UnitOfWork SetUpDbContext(ChatDbContext newDbContext) 
        {
            return new UnitOfWork(newDbContext, _serviceProvider);
        }

        T IUnitOfWork.GetRepository<T>()
        {
            var typeName = typeof(T).Name;
            if (!_repositories.ContainsKey(typeName))
            {
                T repository = _serviceProvider.GetService<T>() 
                    ?? throw new ArgumentNullException($"Repository {typeName} doesn't exist");
                _repositories.Add(typeName, repository);
            }
            return (T)_repositories[typeName];
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
