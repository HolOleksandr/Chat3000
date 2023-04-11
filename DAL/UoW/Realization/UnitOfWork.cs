using DAL.Data;
using DAL.Repositories.Interfaces;
using DAL.Repositories.Realizations;
using DAL.UoW.Interface;

namespace DAL.UoW.Realization
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChatDbContext _dbContext;
        private IUserRepository? _usersRepository;
        private IMessageRepository? _messagesRepository;


        public UnitOfWork(ChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUserRepository UsersRepository
        {
            get
            {
                _usersRepository ??= new UserRepository(_dbContext);
                return _usersRepository;
            }
        }

        public IMessageRepository MessagesRepository
        {
            get
            {
                _messagesRepository ??= new MessageRepository(_dbContext);
                return _messagesRepository;
            }
        }

        public async Task<bool> SaveAsync()
        {
            bool returnValue = true;
            using var dbContextTransaction = _dbContext.Database.BeginTransaction();
            try
            {
                await _dbContext.SaveChangesAsync();
                dbContextTransaction.Commit();
            }
            catch (Exception)
            {
                returnValue = false;
                dbContextTransaction?.Rollback();
            }
            return returnValue;
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
