using AuthenticationService.Application.Interfaces.Strategies.Delete;
using AuthenticationService.Application.Interfaces.Strategies.Delete.Factories;
using AuthenticationService.Application.Strategies.Delete.Enums;
using AuthenticationService.Domain.Interfaces.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Strategies.Delete.Factories
{
    public class DeleteStrategyFactory : IDeleteStrategyFactory
    {
        public IServiceProvider _serviceProvider { get; }

        public DeleteStrategyFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IDeletionStrategy<T> Create<T>(DeletionMode deletionMode) where T : class, IBaseDomainModel
        {
            var deletionStrategy = _serviceProvider.GetServices<IDeletionStrategy<T>>().First(x => x.DeletionMode == deletionMode);
            return deletionStrategy;
        }
    }
}
