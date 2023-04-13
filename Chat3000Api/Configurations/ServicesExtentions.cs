using AutoMapper;
using Chat.BLL.Automapper;
using Chat.BLL.Services.Interfaces;
using Chat.BLL.Services.Realizations;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;
using Chat.DAL.Repositories.Realizations;
using Chat.DAL.UoW.Interface;
using Chat.DAL.UoW.Realization;

namespace ChatApp.API.Configurations
{
    public static class ServicesExtentions
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<IMessageRepository, MessageRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IUserService, UserService>();

        }

        public static void ConfigureMapping(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutomapperProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
