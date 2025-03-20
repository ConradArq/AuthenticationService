using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AuthenticationService.Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationService.Infrastructure.Persistence.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {

            bool anyRecords = builder.Metadata.GetSeedData().Any();

            if (!anyRecords)
            {
                var admin = new ApplicationUser
                {
                    Id = "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                    UserName = "conraarq",
                    NormalizedUserName = "CONRAARQ",
                    FirstName = "Conrado",
                    LastName = "Arquer",
                    Email = "conra.arq@gmail.com",
                    NormalizedEmail = "CONRA.ARQ@GMAIL.COM",
                    PhoneNumber = "555-555",
                    EmailConfirmed = true,
                    IsRegistered = true,
                    PhoneNumberConfirmed = true,
                    StatusId = (int)Domain.Enums.Status.Active,
                    CreatedDate = new DateTime(2025,01,01),
                    CreatedBy = "a18be9c0-aa65-4af8-bd17-00bd9344e575"
                };

                admin.PasswordHash = GeneratePassword(admin);
                builder.HasData(admin);

                builder.HasMany(x => x.ApplicationUserRoles)
                    .WithOne(x => x.ApplicationUser)
                    .HasForeignKey(x => x.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(user => user.Status)
                    .WithMany()
                    .HasForeignKey(user => user.StatusId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }

        public string GeneratePassword(ApplicationUser user)
        {
            var passHash = new PasswordHasher<ApplicationUser>();
            return passHash.HashPassword(user, "AuthenticationService2025$");
        }
    }
}
