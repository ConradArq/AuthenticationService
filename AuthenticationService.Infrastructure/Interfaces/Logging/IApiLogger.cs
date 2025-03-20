using AuthenticationService.Infrastructure.Logging.Models;

namespace AuthenticationService.Infrastructure.Interfaces.Logging
{
    public interface IApiLogger
    {
        void LogInfo(AuditLog logEntry);

        void LogError(ErrorLog logEntry);
    }
}
