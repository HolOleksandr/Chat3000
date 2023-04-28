using Chat.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DAL.Configurations
{
    public class GroupInfoViewConfiguration : IEntityTypeConfiguration<GroupInfoView>
    {
        public void Configure(EntityTypeBuilder<GroupInfoView> builder)
        {
            builder.ToView("GroupView");
            builder.HasKey(x => x.Id);
        }
    }
}
