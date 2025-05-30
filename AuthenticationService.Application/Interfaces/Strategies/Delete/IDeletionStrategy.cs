﻿using AuthenticationService.Application.Strategies.Delete.Enums;
using AuthenticationService.Domain.Interfaces.Models;
using AuthenticationService.Domain.Interfaces.Repositories;

namespace AuthenticationService.Application.Interfaces.Strategies.Delete
{
    /// <summary>
    /// Defines a strategy for deleting an entity, which may be implemented as a hard or soft delete.
    /// </summary>
    public interface IDeletionStrategy<T> where T : class, IBaseDomainModel
    {
        DeletionMode DeletionMode { get; }
        void Delete(T entity, IUnitOfWork unitOfWork);
    }
}
