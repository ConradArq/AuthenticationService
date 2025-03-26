using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Retrieves data with optional filtering, joining, ordering, grouping, projection, and eager loading.
        /// This method provides extensive query customization capabilities.
        /// </summary>
        /// <typeparam name="TOther">The second entity type used for performing a join.</typeparam>
        /// <typeparam name="TResult">The type of the final result after applying the query transformations.</typeparam>
        /// <param name="predicate">
        ///     (Optional) A filter expression to apply before executing the query.
        ///     - Example: `x => x.IsActive` (retrieves only active records).
        /// </param>
        /// <param name="join">
        ///     (Optional) A function defining how to join the primary entity (`T`) with another entity (`TOther`).
        ///     - Example: `(users, orders) => users.Join(orders, user => user.Id, order => order.UserId, 
        ///       (user, order) => new { user.Name, order.Total })`
        /// </param>
        /// <param name="includesAndThenIncludes">
        ///     (Optional) A dictionary specifying related entities to include.
        ///     - The **key** (`Expression<Func<T, object>>`) is the primary navigation property to include.
        ///     - The **value** (`List<Expression<Func<object, object>>>?`) contains **nested navigation properties** (via `.ThenInclude()`).
        ///     
        ///     **Example Usage:**
        ///     
        ///     new Dictionary<Expression<Func<T, object>>, List<Expression<Func<object, object>>>?>
        ///     { 
        ///         { x => x.NavigationProperty1, null }, // Simple Include
        ///         { x => x.NavigationProperty2, new List<Expression<Func<object, object>>>
        ///             { x => ((TRelatedEntity)x).SubNavigationProperty } } // Include + ThenInclude
        ///     }
        /// </param>
        /// <param name="orderBy">
        ///     (Optional) A function to apply sorting to the query.
        ///     - Example: `query => query.OrderBy(x => x.Name)`.
        /// </param>
        /// <param name="groupBy">
        ///     (Optional) A function to group the results.
        ///     - Example: `query => query.GroupBy(x => x.CategoryId)`.
        /// </param>
        /// <param name="having">
        ///     (Optional) A filter applied to grouped results.
        ///     - Example: `group => group.Count() > 5` (retrieves only groups containing more than 5 elements).
        /// </param>
        /// <param name="select">
        ///     (Optional) A projection function to transform the result into a different structure.
        ///     - Example: `query => query.Select(x => new Dto { Id = x.Id, Name = x.Name })`.
        /// </param>
        /// <param name="disableTracking">
        ///     (Optional, default: `true`) If `true`, disables Entity Framework's change tracking (`AsNoTracking()`), 
        ///     which improves performance for read-only queries.
        /// </param>
        /// <returns>
        ///     An `IQueryable<TResult>` representing the query, allowing further refinement before execution.
        /// </returns>
        IQueryable<TResult> Get<TOther, TResult>(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IQueryable<TOther>, IQueryable<TResult>>? join = null,
            Dictionary<Expression<Func<T, object>>, List<Expression<Func<object, object>>>?>? includesAndThenIncludes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IQueryable<IGrouping<object, T>>>? groupBy = null,
            Expression<Func<IGrouping<object, T>, bool>>? having = null,
            Func<IQueryable<TResult>, IQueryable<TResult>>? select = null,
            bool disableTracking = true)
            where TOther : class;

        /// <summary>
        /// A simplified wrapper around <see cref="Get{TOther, TResult}"/> that retrieves entities of type `T` without requiring joins, 
        /// grouping, or projection. Uses the default interface method implementation.
        /// </summary>
        /// <inheritdoc cref="Get{TOther, TResult}"/>
        IQueryable<T> Get(
            Expression<Func<T, bool>>? predicate = null,
            Dictionary<Expression<Func<T, object>>, List<Expression<Func<object, object>>>?>? includesAndThenIncludes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool disableTracking = true)
                {
                    return Get<T, T>(
                        predicate: predicate,
                        join: null,
                        includesAndThenIncludes: includesAndThenIncludes,
                        orderBy: orderBy,
                        groupBy: null,
                        having: null,
                        select: null,
                        disableTracking: disableTracking
                    );
                }

        /// <summary>
        /// Retrieves a list of entities from the database with optional filtering, ordering, and eager loading.
        /// </summary>
        /// <typeparam name="T">The entity type being queried.</typeparam>
        /// <param name="predicate">
        ///     (Optional) A filter expression to apply toquery. Example: `x => x.IsActive` to retrieve only active records.
        /// </param>
        /// <param name="includesAndThenIncludes">
        ///     (Optional) A dictionary specifying related entities to include.
        ///     - The **key** (`Expression<Func<T, object>>`) is the primary navigation property to include.
        ///     - The **value** (`List<Expression<Func<object, object>>>?`) contains **nested navigation properties** (via `.ThenInclude()`).
        ///     
        ///     **Example Usage:**
        ///     
        ///     new Dictionary<Expression<Func<T, object>>, List<Expression<Func<object, object>>>?>
        ///     { 
        ///         { x => x.NavigationProperty1, null }, // Simple Include
        ///         { x => x.NavigationProperty2, new List<Expression<Func<object, object>>>
        ///             { x => ((TRelatedEntity)x).SubNavigationProperty } } // Include + ThenInclude
        ///     }
        /// </param>
        /// <param name="orderBy">
        ///     (Optional) A function to apply ordering to the query. Example: `query => query.OrderBy(x => x.Name)`.
        /// </param>
        /// <param name="disableTracking">
        ///     (Optional, default: `true`) Whether to disable tracking (`AsNoTracking()`) for read-only queries.
        /// </param>
        /// <returns>
        ///     A **read-only list** of the requested entity type (`IReadOnlyList<T>`) from the database.
        /// </returns>
        Task<IReadOnlyList<T>> GetAsync(
            Expression<Func<T, bool>>? predicate = null,
            Dictionary<Expression<Func<T, object>>, List<Expression<Func<object, object>>>?>? includesAndThenIncludes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool disableTracking = true);

        /// <summary>
        /// Retrieves a paginated list of entities from the database.
        /// Works like <see cref="GetAsync"/> but adds pagination.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="pageNumber">The page number (1-based index). Defaults to 1 if less than or equal to 0.</param>
        /// <param name="pageSize">The number of records per page. Must be greater than 0.</param>
        /// <returns>
        ///     A tuple containing:
        ///     - **Data**: The paginated list of entities.
        ///     - **TotalItems**: The total number of records matching the criteria.
        /// </returns>
        Task<(IReadOnlyList<T> Data, int TotalItems)> GetPaginatedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>>? predicate = null,
            Dictionary<Expression<Func<T, object>>, List<Expression<Func<object, object>>>?>? includesAndThenIncludes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool disableTracking = true);

        /// <summary>
        /// Retrieves a single entity by its ID.
        /// Works like <see cref="GetAsync"/> but fetches only one entity.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>The entity if found; otherwise, `null`.</returns>
        Task<T?> GetSingleAsync(
            object id,
            Dictionary<Expression<Func<T, object>>, List<Expression<Func<object, object>>>?>? includesAndThenIncludes = null,
            bool disableTracking = false);

        /// <summary>
        /// Creates a new entity in the database.
        /// </summary>
        /// <param name="entity">The entity to be created.</param>
        /// <returns>The created entity.</returns>
        T Create(T entity);

        /// <summary>
        /// Creates multiple entities in the database in a single transaction.
        /// </summary>
        /// <param name="entities">The collection of entities to create.</param>
        /// <returns>The created entities.</returns>
        ICollection<T> CreateRange(ICollection<T> entities);

        /// <summary>
        /// Deletes an entity from the database.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        void Delete(T entity);

        /// <summary>
        /// Deletes multiple entities from the database in a single transaction.
        /// </summary>
        /// <param name="entities">The collection of entities to delete.</param>
        void DeleteRange(ICollection<T> entities);

        /// <summary>
        /// Updates an entity in the database.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>The updated entity.</returns>
        T Update(T entity);

        /// <summary>
        /// Updates multiple entities in a single transaction.
        /// </summary>
        /// <param name="entities">The collection of entities to update.</param>
        /// <returns>The updated entities.</returns>
        ICollection<T> UpdateRange(ICollection<T> entities);

        /// <summary>
        /// Reloads the specified entity from the database, refreshing its values.
        /// Optionally, a specific property can be reloaded by providing the property expression.
        /// </summary>
        /// <param name="entity">The entity to reload from the database.</param>
        /// <param name="property">Optional expression to specify a particular property to reload. If null, the entire entity is reloaded.</param>
        /// <typeparam name="TProperty">The type of the property to reload, if specified.</typeparam>
        void Reload<TProperty>(T entity, Expression<Func<T, TProperty?>>? property = null) where TProperty : class;
    }
}
