﻿using AutoMapper;
using Chat.BLL.Automapper;
using Chat.BLL.Services.Implementation;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Data;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Implementation;
using Chat.DAL.Repositories.Interfaces;
using Chat.DAL.UoW.Implementation;
using Chat.DAL.UoW.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
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
        private UserManager<User> _userManager;

        public IUserAuthService GetUserAuthService()
        {

            var _UoW = InitUnitOfWorkWithContext();
            var config = GetConfiguration();
            _userManager = _serviceProvider.GetService<UserManager<User>>();
            IUserAuthService _userService = new UserAuthService(_userManager, config, CreateMapperProfile());
            return _userService;
        }

        public IMessageService GetMessageService()
        {
            var _UoW = InitUnitOfWorkWithContext();
            IMessageService _messageService = new MessageService(_UoW, CreateMapperProfile());
            return _messageService;
        }
        public IUserService GetUserService()
        {
            var _UoW = InitUnitOfWorkWithContext();
            IUserService _userService = new UserService(_UoW, CreateMapperProfile());
            return _userService;
        }

        public IGroupService GetGroupService()
        {
            var _UoW = InitUnitOfWorkWithContext();
            IGroupService _userService = new GroupService(_UoW, CreateMapperProfile());
            return _userService;
        }

        private IUnitOfWork InitUnitOfWorkWithContext()
        {
            _chatDbContext = GetDbContext();
            SetUpServices();
            var _UoW = GetUoW();
            return _UoW;
        }

        private static IUnitOfWork GetUoW()
        {
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

        public static IConfiguration GetConfiguration() =>
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.tests.json", true, true)
                .Build();

        public static void ResetUserTable()
        {
            using var transaction = _chatDbContext.Database.BeginTransaction();
            var query = "DELETE FROM [dbo].[AspNetUsers]";
            _chatDbContext.Database.ExecuteSqlRaw(query);

            _chatDbContext.Users.AddRange(DataSeed.GetfakeUserList());
            _chatDbContext.SaveChanges();
            transaction.Commit();
        }

        public static void ResetGroupTable()
        {
            using var transaction = _chatDbContext.Database.BeginTransaction();
            var query = "DELETE FROM [dbo].[Groups]";
            _chatDbContext.Database.ExecuteSqlRaw(query);

            _chatDbContext.Groups.AddRange(DataSeed.GetfakeGroupList());
            _chatDbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Groups] ON;");
            _chatDbContext.SaveChanges();
            _chatDbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Groups] OFF;");
            transaction.Commit();
        }

        public static void ResetUserGroupTable()
        {
            using var transaction = _chatDbContext.Database.BeginTransaction();
            var query = "DELETE FROM [dbo].[UserGroup]";
            _chatDbContext.Database.ExecuteSqlRaw(query);

            _chatDbContext.UserGroup.AddRange(DataSeed.GetfakeUserGroupList());
            _chatDbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[UserGroup] ON;");
            _chatDbContext.SaveChanges();
            _chatDbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[UserGroup] OFF;");
            transaction.Commit();
        }

        public static void ResetMessageTable()
        {
            using var transaction = _chatDbContext.Database.BeginTransaction();
            var query = "DELETE FROM [dbo].[Messages]";
            _chatDbContext.Database.ExecuteSqlRaw(query);

            _chatDbContext.Messages.AddRange(DataSeed.GetfakeMesagesList());
            _chatDbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Messages] ON;");
            _chatDbContext.SaveChanges();
            _chatDbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Messages] OFF;");
            transaction.Commit();
        }

        private static void SetUpServices()
        {
            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddScoped<IUserRepository, UserRepository>();
            _serviceCollection.AddScoped<IGroupRepository, GroupRepository>();
            _serviceCollection.AddScoped<IUserGroupRepository, UserGroupRepository>();
            _serviceCollection.AddScoped<IMessageRepository, MessageRepository>();
            _serviceCollection.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ChatDbContext>()
                .AddDefaultTokenProviders();
            _serviceCollection.AddLogging(config =>
            {
                config.AddDebug();
                config.AddConsole();
            });
            //_serviceCollection.AddScoped<ILogger<UserManager<User>>, Logger<UserManager<User>>>();
            // _serviceCollection.AddScoped<ILoggerFactory, LoggerFactory>();

            _serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            _serviceCollection.AddScoped(_ => _chatDbContext);
            var serviceProviderFactory = new DefaultServiceProviderFactory();
            //_serviceProvider = _serviceCollection.BuildServiceProvider();
            _serviceProvider = serviceProviderFactory.CreateServiceProvider(_serviceCollection);
        }

        private static IMapper CreateMapperProfile()
        {
            var myProfile = new AutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }
        public static void ConfigureIdentity(IServiceCollection services)
        {
            


            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
            });
        }

        public virtual void Dispose()
        {
            _chatDbContext.Dispose();
        }

    }
}
