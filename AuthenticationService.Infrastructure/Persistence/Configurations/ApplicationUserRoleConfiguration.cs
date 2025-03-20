using AuthenticationService.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Persistence.Configurations
{
    public class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
        {
            builder.HasData(

               //Admin
               new ApplicationUserRole()
               {
                   RoleId = "d2674562-193a-41e6-9a92-7f7cb04caf90",
                   UserId = "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                   StatusId = (int)Domain.Enums.Status.Active,
                   CreatedDate = new DateTime(2025, 01, 01),
                   CreatedBy = "a18be9c0-aa65-4af8-bd17-00bd9344e575"
               }
           );
        }
    }
}
