using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using AuthenticationService.Domain.Interfaces.Models;
using AuthenticationService.Domain.Interfaces.Repositories;

namespace AuthenticationService.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IBaseDomainModel
    {
        protected readonly AuthenticationServiceDbContext _context;

        public GenericRepository(AuthenticationServiceDbContext context)
        {
            _context = context;
        }

        public virtual IQueryable<TResult> Get<TOther, TResult>(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IQueryable<TOther>, IQueryable<TResult>>? join = null,
            Dictionary<Expression<Func<T, object>>, List<Expression<Func<object, object>>>?>? includesAndThenIncludes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IQueryable<IGrouping<object, T>>>? groupBy = null,
            Expression<Func<IGrouping<object, T>, bool>>? having = null,
            Func<IQueryable<TResult>, IQueryable<TResult>>? select = null,
            bool disableTracking = true)
            where TOther : class
        {
            throw new NotImplementedException();

            //TODO: Implement method

            //IQueryable<T> query = _context.Set<T>();
            //IQueryable<TOther> otherQuery = _context.Set<TOther>();

            //// Disable Tracking
            //if (disableTracking)
            //    query = query.AsNoTracking();

            //// Add Related Entities
            //if (includesAndThenIncludes != null)
            //{
            //    query = includesAndThenIncludes.Aggregate(query, (current, x) =>
            //    {
            //        var queryWithInclude = current.Include(x.Key);

            //        if (x.Value != null)
            //        {
            //            foreach (var thenInclude in x.Value)
            //            {
            //                if (thenInclude != null)
            //                    queryWithInclude = queryWithInclude.ThenInclude(thenInclude);
            //            }
            //        }
            //        return queryWithInclude;
            //    });
            //}

            //// Apply Filter
            //if (predicate != null)
            //    query = query.Where(predicate);

            //// Apply Join
            //IQueryable<TResult> resultQuery;
            //if (join != null)
            //    resultQuery = join(query, otherQuery);
            //else
            //    resultQuery = (IQueryable<TResult>)(object)query;

            //// Apply GroupBy and Having
            //if (groupBy != null)
            //{
            //    var groupedQuery = groupBy((IQueryable<T>)(object)resultQuery);
            //    if (having != null)
            //        groupedQuery = groupedQuery.Where(having);

            //    resultQuery = (IQueryable<TResult>)(object)groupedQuery;
            //}

            //// Apply OrderBy
            //if (orderBy != null)
            //    resultQuery = (IQueryable<TResult>)(object)orderBy((IQueryable<T>)(object)resultQuery);

            //// Apply Select (Projection)
            //if (select != null)
            //    resultQuery = select(resultQuery);

            //return resultQuery;
        }

        public virtual async Task<IReadOnlyList<T>> GetAsync(
            Expression<Func<T, bool>>? predicate = null,
            Dictionary<Expression<Func<T, object>>, List<Expression<Func<object, object>>>?>? includesAndThenIncludes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();
            if (disableTracking) query = query.AsNoTracking();

            if (includesAndThenIncludes != null)
            {
                query = includesAndThenIncludes.Aggregate(query, (current, x) =>
                {
                    var queryWithInclude = current.Include(x.Key);

                    if (x.Value != null)
                    {
                        foreach (var thenInclude in x.Value)
                        {
                            if (thenInclude != null)
                                queryWithInclude = queryWithInclude.ThenInclude(thenInclude);
                        }
                    }
                    return queryWithInclude;
                });
            }

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = orderBy(query);

            return await query.ToListAsync();
        }

        public virtual async Task<(IReadOnlyList<T> Data, int TotalItems)> GetPaginatedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>>? predicate = null,
            Dictionary<Expression<Func<T, object>>, List<Expression<Func<object, object>>>?>? includesAndThenIncludes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool disableTracking = true)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) throw new Exception("Page size must be greater than 0");

            IQueryable<T> query = _context.Set<T>();

            if (disableTracking)
                query = query.AsNoTracking();

            if (includesAndThenIncludes != null)
            {
                query = includesAndThenIncludes.Aggregate(query, (current, x) =>
                {
                    var queryWithInclude = current.Include(x.Key);

                    if (x.Value != null)
                    {
                        foreach (var thenInclude in x.Value)
                        {
                            if (thenInclude != null)
                                queryWithInclude = queryWithInclude.ThenInclude(thenInclude);
                        }
                    }

                    return queryWithInclude;
                });
            }

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = orderBy(query);

            var totalItems = await query.CountAsync();

            var paginatedItems = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (paginatedItems, totalItems);
        }

        public virtual async Task<T?> GetSingleAsync(
            object id,
            Dictionary<Expression<Func<T, object>>, List<Expression<Func<object, object>>>?>? includesAndThenIncludes = null,
            bool disableTracking = false)
        {
            IQueryable<T> query = _context.Set<T>();
            if (disableTracking) query = query.AsNoTracking();

            if (includesAndThenIncludes != null)
            {
                query = includesAndThenIncludes.Aggregate(query, (current, x) =>
                {
                    var queryWithInclude = current.Include(x.Key);

                    if (x.Value != null)
                    {
                        foreach (var thenInclude in x.Value)
                        {
                            if (thenInclude != null)
                                queryWithInclude = queryWithInclude.ThenInclude(thenInclude);
                        }
                    }
                    return queryWithInclude;
                });
            }

            T? result = await query.FirstOrDefaultAsync(cd => cd.Id == id);
            return result;
        }

        public virtual T Create(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity;
        }

        public virtual ICollection<T> CreateRange(ICollection<T> entities)
        {
            foreach (var entity in entities)
            {
                _context.Set<T>().Add(entity);
            }
            return entities;
        }

        public virtual T Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return entity;
        }

        public virtual ICollection<T> UpdateRange(ICollection<T> entities)
        {
            foreach (var entity in entities)
            {
                _context.Set<T>().Update(entity);
            }
            return entities;
        }

        public virtual void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public virtual void DeleteRange(ICollection<T> entities)
        {
            foreach (var entity in entities)
            {
                _context.Set<T>().Remove(entity);
            }
        }

        public virtual void Reload<TProperty>(T entity, Expression<Func<T, TProperty?>>? property = null) where TProperty : class
        {
            var entry = _context.Entry(entity);

            if (property == null)
            {
                // Reload the entire entity itself
                entry.Reload();
            }
            else if (typeof(TProperty).IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(typeof(TProperty).GetGenericTypeDefinition()))
            {
                // Reload a collection navigation property
                var collectionProperty = property as Expression<Func<T, IEnumerable<TProperty>>>;

                if (collectionProperty != null)
                {
                    entry.Collection(collectionProperty).Load();
                }
            }
            else
            {
                // Reload a reference navigation property
                entry.Reference(property).Load();
            }
        }
    }
}
