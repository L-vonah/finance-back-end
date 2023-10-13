namespace Finance.Infrastructure;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> FirstAsync(int id);
    Task<IEnumerable<TEntity>> ToListAsync();
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    Task UpdateAsync(TEntity entity);
    Task UpdateRangeAsync(IEnumerable<TEntity> entities);
    Task RemoveAsync(TEntity entity);
    Task RemoveRangeAsync(IEnumerable<TEntity> entities);
}
