using Azure.Identity;
using Chat.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.API.Configurations
{
    public static class BuilderExtentions
    {
        public static void SetUpKeyVaultConfig(this WebApplicationBuilder builder)
        {
            string? keyVaultName;
            if (builder.Environment.IsProduction())
            {
                keyVaultName = builder.Configuration["KeyVaultName"];
            }
            else
            {
                keyVaultName = builder.Configuration["DefaultKeyVaultName"];
            }
            var keyVaultEndpoint = new Uri($"https://{keyVaultName}.vault.azure.net/");
            var credential = new DefaultAzureCredential();
            builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, credential);
        }
        
        public static void ConfigureDbContext(this WebApplicationBuilder builder)
        {
            var connectionString = "";
            if (builder.Environment.IsProduction())
            {
                connectionString = builder.Configuration.GetConnectionString("ProductionDbConnection");
            }
            else
            {
                connectionString = builder.Configuration.GetConnectionString("Default");
            }

            builder.Services.AddDbContext<ChatDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }

        public static void ConfigureSignalRConnection(this WebApplicationBuilder builder)
        {
            if (builder.Environment.IsProduction())
            {
                var azureConnectionString = builder.Configuration["Azure:SignalR:ConnectionString"];
                builder.Services.AddSignalR()
                    .AddAzureSignalR(options =>
                    {
                        options.ConnectionString = azureConnectionString;
                    });
            }
            else
            {
                builder.Services.AddSignalR(o =>
                {
                    o.EnableDetailedErrors = true;
                    o.MaximumReceiveMessageSize = long.MaxValue;
                });
            }
        }

    }
}
