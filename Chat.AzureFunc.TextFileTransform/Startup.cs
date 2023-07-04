using Chat.AzureFunc.TextFileTransform;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Chat.AzureFunc.TextFileTransform.Configuration;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Chat.AzureFunc.TextFileTransform
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.KeyVaultConfigValues();
            builder.ConfigureJWTAuth();
            builder.ConfigureDbContext();
            builder.ConfigureServices();
        }
    }
}
