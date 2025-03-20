using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using AuthenticationService.Shared.Extensions;
using AuthenticationService.Infrastructure.Persistence.Configurations;
using AuthenticationService.Infrastructure.Logging.Models.Enums;
using AuthenticationService.Infrastructure.Logging.Models;
using AuthenticationService.Domain.Models.Entities;
using AuthenticationService.Infrastructure.Interfaces.Logging;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AuthenticationService.Domain.Interfaces.Models;

namespace AuthenticationService.Infrastructure.Persistence
{
    //We use full generic arguments to prevent EF from assuming TPH
    public class AuthenticationServiceDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string,
        IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public string? CurrentUserId { get; set; }
        private readonly IApiLogger _apiLogger;

        public AuthenticationServiceDbContext(DbContextOptions<AuthenticationServiceDbContext> options, IApiLogger apiLogger) : base(options)
        {
            _apiLogger = apiLogger;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<IBaseDomainModel>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now.InTimeZone();
                        entry.Entity.CreatedBy = CurrentUserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now.InTimeZone();
                        entry.Entity.LastModifiedBy = CurrentUserId;
                        break;
                }
            }

            var changedEntities = ChangeTracker.Entries<IBaseDomainModel>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .Select(entry => new
                {
                    Entry = entry,
                    OriginalState = entry.State,
                    EntityName = Model.FindEntityType(entry.Entity.GetType())?.GetTableName(),
                    Schema = Model.FindEntityType(entry.Entity.GetType())?.GetSchema(),
                    OldValues = JsonSerializer.Serialize(entry.Properties.Select(property => new Dictionary<string, object?>()
                    {
                        { property.Metadata.Name, property.OriginalValue }
                    }))
                })
                .ToList();

            int result = await base.SaveChangesAsync(cancellationToken);

            foreach (var changedEntity in changedEntities)
            {
                var auditLog = new AuditLog
                {
                    EntityName = $"{(changedEntity.Schema != null ? string.Concat(changedEntity.Schema, ".") : string.Empty)}{changedEntity.EntityName}"
                };

                switch (changedEntity.OriginalState)
                {
                    case EntityState.Added:
                        auditLog.EventType = EventType.Create;
                        auditLog.NewData = JsonSerializer.Serialize(changedEntity.Entry.Properties.Select(property => new Dictionary<string, object?>()
                        {
                            { property.Metadata.Name, property.CurrentValue }
                        }));
                        break;

                    case EntityState.Modified:
                        auditLog.EventType = EventType.Update;
                        auditLog.OldData = changedEntity.OldValues;
                        auditLog.NewData = JsonSerializer.Serialize(changedEntity.Entry.Properties.Select(property => new Dictionary<string, object?>()
                        {
                            { property.Metadata.Name, property.CurrentValue }
                        }));
                        break;

                    case EntityState.Deleted:
                        auditLog.EventType = EventType.Delete;
                        auditLog.OldData = changedEntity.OldValues;
                        break;
                }

                _apiLogger.LogInfo(auditLog);
            }

            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("AuthenticationService");

            builder.ApplyConfiguration(new ApplicationMenuConfiguration());
            builder.ApplyConfiguration(new ApplicationRoleConfiguration());
            builder.ApplyConfiguration(new ApplicationRoleMenuConfiguration());
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new ApplicationUserRoleConfiguration());
            builder.ApplyConfiguration(new RefreshTokenConfiguration());
            builder.ApplyConfiguration(new StatusConfiguration());
        }

        public DbSet<ApplicationMenu> ApplicationMenu { get; set; }
        public DbSet<ApplicationRoleMenu> ApplicationRoleMenu { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Status> Status { get; set; }
    }
}
