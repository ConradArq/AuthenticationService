using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationService.Domain.Models.Entities;

namespace AuthenticationService.Infrastructure.Persistence.Configurations
{
    public class ApplicationMenuConfiguration : IEntityTypeConfiguration<ApplicationMenu>
    {
        public void Configure(EntityTypeBuilder<ApplicationMenu> builder)
        {
            builder.HasData(
                new ApplicationMenu
                {
                    Id = 1,
                    Title = "MENU",
                    GroupTitle = true,
                    CreatedDate = new DateTime(2025, 01, 01),
                    CreatedBy = "a18be9c0-aa65-4af8-bd17-00bd9344e575"
                },
                new ApplicationMenu
                {
                    Id = 2,
                    Title = "Calendar",
                    IconType = "feather",
                    Icon = "calendar",
                    Class = "menu-toggle",
                    GroupTitle = false,
                    CreatedDate = new DateTime(2025, 01, 01),
                    CreatedBy = "a18be9c0-aa65-4af8-bd17-00bd9344e575"
                }
            );

            builder.HasMany(x => x.ApplicationRoleMenus)
                .WithOne(x => x.ApplicationMenu)
                .HasForeignKey(x => x.ApplicationMenuId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.ApplicationSubMenus)
                .WithOne(x => x.ParentApplicationMenu)
                .HasForeignKey(x => x.ParentApplicationMenuId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(user => user.Status)
                .WithMany()
                .HasForeignKey(user => user.StatusId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
