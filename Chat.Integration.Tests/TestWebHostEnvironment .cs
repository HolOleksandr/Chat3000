using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Chat.Integration.Tests
{
    public class TestWebHostEnvironment : IWebHostEnvironment
    {
        public string EnvironmentName { get; set; } = "Development";
        public string ApplicationName { get; set; } = "TestApplication";
        public string WebRootPath { get; set; } = "wwwroot";
        public IFileProvider WebRootFileProvider { get; set; }
        public string ContentRootPath { get; set; } = Directory.GetCurrentDirectory();
        public IFileProvider ContentRootFileProvider { get; set; }
    }
}
