using Chat.DAL.Configurations;
using Chat.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chat.DAL.Data
{
    public class ChatDbContext : IdentityDbContext<User>
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroup { get; set; }
        public DbSet<GroupInfoView> GroupsInfo { get; set; }
        public DbSet<PdfContract> PdfContract { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new GroupInfoViewConfiguration().Configure(modelBuilder.Entity<GroupInfoView>());

            modelBuilder.Entity<Group>()
                .HasMany(g => g.Users)
                .WithMany(u => u.Groups)
                .UsingEntity<UserGroup>();

            modelBuilder.Entity<Group>()
                .HasMany(g => g.Messages)
                .WithOne(m => m.Group)
                .HasForeignKey(m => m.GroupId);

            modelBuilder.Entity<Group>()
                .HasOne(g => g.Admin)
                .WithMany(a => a.AdminInGroups)
                .HasForeignKey(g => g.AdminId);

            modelBuilder.Entity<Group>()
                .HasOne(c => c.GroupInfo)
                .WithOne(ci => ci.Group)
                .HasForeignKey<GroupInfoView>(ci => ci.Id);

            modelBuilder.Entity<PdfContract>()
                .HasOne(g => g.Uploader)
                .WithMany(m => m.UploadedPdfContracts)
                .HasForeignKey(m => m.UploaderId);

            modelBuilder.Entity<PdfContract>()
                .HasOne(g => g.Receiver)
                .WithMany(m => m.ReceivedPdfContracts)
                .HasForeignKey(m => m.ReceiverId);
        }
    }
}
