using System.Linq.Expressions;

namespace Finance.Infrastructure;

/// <summary>
/// See <a href="https://www.c-sharpcorner.com/article/net-entity-framework-core-generic-async-operations-with-unit-of-work-generic-re/"/>
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IRepository<TEntity> where TEntity : class // TODO: Add constraint for IEntity like IAggregateRoot. Move to Infrastructure project.
{
    Task<TEntity> FindAsync(int id);
    Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> match);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> match);
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    Task UpdateAsync(TEntity entity);
    Task UpdateRangeAsync(IEnumerable<TEntity> entities);
    Task RemoveAsync(TEntity entity);
    Task RemoveRangeAsync(IEnumerable<TEntity> entities);
    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
    IQueryable<TEntity> WhereIf(bool condition, Expression<Func<TEntity, bool>> predicate);

}
// https://www.c-sharpcorner.com/article/net-entity-framework-core-generic-async-operations-with-unit-of-work-generic-re/