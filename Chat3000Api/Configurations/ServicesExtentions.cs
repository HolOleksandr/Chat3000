using AutoMapper;
using BAL.Automapper;
using BAL.Services.Interfaces;
using BAL.Services.Realizations;
using DAL.UoW.Interface;
using DAL.UoW.Realization;

namespace Chat3000Api.Configurations
{
    public static class ServicesExtentions
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
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
