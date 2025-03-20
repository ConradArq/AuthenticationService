using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AuthenticationService.Domain.Models.Entities;

namespace AuthenticationService.Infrastructure.Persistence.Configurations
{
    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData(
                new ApplicationRole
                {
                    Id = "d2674562-193a-41e6-9a92-7f7cb04caf90",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                    StatusId = (int)Domain.Enums.Status.Active,
                    CreatedDate = new DateTime(2025, 01, 01),
                    CreatedBy = "a18be9c0-aa65-4af8-bd17-00bd9344e575"
                },
                new ApplicationRole
                {
                    Id = "b37495f4-c5b4-4cfa-9a34-68f28f0fd6a6",
                    Name = "Manager",
                    NormalizedName = "MANAGER",
                    StatusId = (int)Domain.Enums.Status.Active,
                    CreatedDate = new DateTime(2025, 01, 01),
                    CreatedBy = "a18be9c0-aa65-4af8-bd17-00bd9344e575"
                }
                ,
                new ApplicationRole
                {
                    Id = "1f4b2341-7326-4fca-a906-7c9db3abbd4b",
                    Name = "Guest",
                    NormalizedName = "GUEST",
                    StatusId = (int)Domain.Enums.Status.Active,
                    CreatedDate = new DateTime(2025, 01, 01),
                    CreatedBy = "a18be9c0-aa65-4af8-bd17-00bd9344e575"
                }
            );

            builder.HasMany(x => x.ApplicationUserRoles)
                .WithOne(x => x.ApplicationRole)
                .HasForeignKey(x => x.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.ApplicationRoleMenus)
                .WithOne(x => x.ApplicationRole)
                .HasForeignKey(x => x.ApplicationRoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(user => user.Status)
                .WithMany()
                .HasForeignKey(user => user.StatusId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
