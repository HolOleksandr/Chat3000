using Chat.BLL.Models.AzureFuncModels;
using Microsoft.EntityFrameworkCore;

namespace Chat.AzureFunc.TextFileTransform.FuncDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<DocFileText> DocFileText { get; set; }
    }
}
