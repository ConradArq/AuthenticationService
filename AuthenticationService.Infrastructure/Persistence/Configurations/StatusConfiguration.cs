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
    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.HasData(
                new Status
                {
                    Id = (int)Domain.Enums.Status.Active,
                    Name = "Active",
                    CreatedDate = new DateTime(2025, 01, 01),
                    CreatedBy = "a18be9c0-aa65-4af8-bd17-00bd9344e575"
                },

                new Status
                {
                    Id = (int)Domain.Enums.Status.Inactive,
                    Name = "Inactive",
                    CreatedDate = new DateTime(2025, 01, 01),
                    CreatedBy = "a18be9c0-aa65-4af8-bd17-00bd9344e575"
                }
            );
        }
    }
}
