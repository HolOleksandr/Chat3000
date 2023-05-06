using AutoMapper;
using Chat.BLL.Automapper;
using Chat.BLL.DTO;
using Chat.BLL.Models.Requests;
using Chat.BLL.Services.Interfaces;
using Chat.BLL.Services.Realizations;
using Chat.BLL.Validators;
using Chat.DAL.Data;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;
using Chat.DAL.Repositories.Realizations;
using Chat.DAL.UoW.Interface;
using Chat.DAL.UoW.Realization;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ChatApp.API.Configurations
{
    public static class ServicesExtentions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsDefault", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("content-disposition"));
            });
        }

        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IUserGroupRepository, UserGroupRepository>();



            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserAuthService, UserAuthService>();

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

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ChatDbContext>()
            .AddDefaultTokenProviders();

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




        public static void ConfigureValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CreateGroupRequest>();
            services.AddValidatorsFromAssemblyContaining<UserLoginValidator>();
            services.AddValidatorsFromAssemblyContaining<UserRegistrationValidator>();
            services.AddValidatorsFromAssemblyContaining<UserChangePassValidator>();
            services.AddValidatorsFromAssemblyContaining<UserDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<MessageDTO>();
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "0auth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Chat300API.Api", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);

            });
        }

        public static void ConfigureAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AllUsers",
                    policy => policy.RequireRole("User", "Administrator"));

                options.AddPolicy("UserRole",
                    policy => policy.RequireRole("User"));

                options.AddPolicy("AdministratorRole",
                    policy => policy.RequireRole("Administrator"));

                options.AddPolicy("ModeratorRole",
                    policy => policy.RequireRole("Moderator"));
            });
        }
    }
}
