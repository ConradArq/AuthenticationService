using AuthenticationService.Application.Interfaces.Strategies.Delete;
using AuthenticationService.Application.Strategies.Delete.Enums;
using AuthenticationService.Domain.Interfaces.Models;

namespace AuthenticationService.Application.Interfaces.Strategies.Delete.Factories
{
    public interface IDeleteStrategyFactory
    {
        IServiceProvider _serviceProvider { get; }

        IDeletionStrategy<T> Create<T>(DeletionMode deletionMode) where T : class, IBaseDomainModel;
    }
}