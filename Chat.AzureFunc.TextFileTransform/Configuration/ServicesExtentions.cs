using Azure.Identity;
using Chat.AzureFunc.TextFileTransform.FuncDbContext;
using Chat.AzureFunc.TextFileTransform.Services.Implementation;
using Chat.AzureFunc.TextFileTransform.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Text;

namespace Chat.AzureFunc.TextFileTransform.Configuration
{
    public static class ServicesExtentions
    {
        public static void KeyVaultConfigValues(this IFunctionsHostBuilder builder)
        {
            var keyVaultName = Environment.GetEnvironmentVariable("keyVaultName");
            var keyVaultEndpoint = new Uri($"https://{keyVaultName}.vault.azure.net/");
            var executionContextOptions = builder.Services.BuildServiceProvider().GetService<IOptions<ExecutionContextOptions>>().Value;
            var appDirectory = executionContextOptions.AppDirectory;
            var config = new ConfigurationBuilder()
                .SetBasePath(appDirectory)
                .AddJsonFile(Path.Combine(appDirectory, "local.settings.json"), optional: true, reloadOnChange: true)
                .AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential())
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddSingleton<IConfiguration>(config);
        }

        public static void ConfigureJWTAuth(this IFunctionsHostBuilder builder)
        {
            var key = Environment.GetEnvironmentVariable("JwtKey");
            builder.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        RequireExpirationTime = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Environment.GetEnvironmentVariable("JwtIssuer"),
                        ValidAudience = Environment.GetEnvironmentVariable("JwtAudience"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    };
                });
        }

        public static void ConfigureDbContext(this IFunctionsHostBuilder builder)
        {
            var configuration = builder.Services.BuildServiceProvider().GetService<IConfiguration>();

            var connectionString = configuration.GetConnectionString("ProductionDbConnection");
            builder.Services.AddDbContext<AppDbContext>(
                options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString));
        }

        public static void ConfigureServices(this IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<ITextInfoService, TextInfoService>();
        }
    }
}
