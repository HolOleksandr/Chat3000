using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DAL.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
    }
}
