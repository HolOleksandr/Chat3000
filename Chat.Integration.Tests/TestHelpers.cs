using AutoMapper;
using Chat.BLL.Automapper;
using Chat.BLL.Services.Implementation;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Data;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Implementation;
using Chat.DAL.Repositories.Interfaces;
using Chat.DAL.UoW.Implementation;
using Chat.DAL.UoW.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Integration.Tests
{
    public class TestHelpers
    {
        private const string TestConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ChatTestDb;Integrated Security=True;";
        private static IServiceProvider _serviceProvider;
        private static IServiceCollection _serviceCollection;
        private static ChatDbContext _chatDbContext;
        private DbContextOptions<ChatDbContext> _options;

        public IUserService GetUserService()
        {
            SetUpServices();
            var _UoW = GetUoW();
            IUserService _userService = new UserService(_UoW, CreateMapperProfile());
            return _userService;
        }

        public IGroupService GetGroupService()
        {
            SetUpServices();
            var _UoW = GetUoW();
            IGroupService _userService = new GroupService(_UoW, CreateMapperProfile());
            return _userService;
        }

        private IUnitOfWork GetUoW()
        {
            _chatDbContext = GetDbContext();
            IUnitOfWork _unitOfWork = new UnitOfWork(_chatDbContext, _serviceProvider);
            return _unitOfWork;
        }
        private ChatDbContext GetDbContext()
        {
            _options = new DbContextOptionsBuilder<ChatDbContext>()
                .UseSqlServer(TestConnectionString)
                .Options;
            return new ChatDbContext(_options);
        }

        public static void ResetUserTable()
        {
            using (var transaction = _chatDbContext.Database.BeginTransaction())
            {
                var query = "DELETE FROM [dbo].[AspNetUsers]";
                _chatDbContext.Database.ExecuteSqlRaw(query);

                _chatDbContext.Users.AddRange(DataSeed.GetfakeUserList());
                _chatDbContext.SaveChanges();
                transaction.Commit();
            }
        }


        public static void ResetGroupTable()
        {
            using (var transaction = _chatDbContext.Database.BeginTransaction())
            {
                var query = "DELETE FROM [dbo].[Groups]";
                _chatDbContext.Database.ExecuteSqlRaw(query);

                _chatDbContext.Groups.AddRange(DataSeed.GetfakeGroupList());
                _chatDbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Groups] ON;");
                _chatDbContext.SaveChanges();
                _chatDbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Groups] OFF;");
                transaction.Commit();
            }
        }

        public static void ResetUserGroupTable()
        {
            using (var transaction = _chatDbContext.Database.BeginTransaction())
            {
                var query = "DELETE FROM [dbo].[UserGroup]";
                _chatDbContext.Database.ExecuteSqlRaw(query);

                _chatDbContext.UserGroup.AddRange(DataSeed.GetfakeUserGroupList());
                _chatDbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[UserGroup] ON;");
                _chatDbContext.SaveChanges();
                _chatDbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[UserGroup] OFF;");
                transaction.Commit();
            }
        }

        public static void ResetMessageTable()
        {
            using (var transaction = _chatDbContext.Database.BeginTransaction())
            {
                var query = "DELETE FROM [dbo].[Messages]";
                _chatDbContext.Database.ExecuteSqlRaw(query); 

                _chatDbContext.Messages.AddRange(DataSeed.GetfakeMesagesList());
                _chatDbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Messages] ON;");
                _chatDbContext.SaveChanges();
                _chatDbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Messages] OFF;");
                transaction.Commit();
            }
        }

        private static void SetUpServices()
        {
            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddTransient<IUserRepository, UserRepository>();
            _serviceCollection.AddTransient<IGroupRepository, GroupRepository>();
            _serviceCollection.AddTransient<IUserGroupRepository, UserGroupRepository>();
            _serviceCollection.AddDbContext<ChatDbContext>(options =>
            {
                options.UseSqlServer(TestConnectionString);
            });
            var serviceProviderFactory = new DefaultServiceProviderFactory();
            _serviceProvider = serviceProviderFactory.CreateServiceProvider(_serviceCollection);
        }

        private static IMapper CreateMapperProfile()
        {
            var myProfile = new AutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }
    }
}
