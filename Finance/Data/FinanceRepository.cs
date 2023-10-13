using Finance.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Finance.Data;

public class FinanceRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly FinanceContext _context;

    public FinanceRepository(FinanceContext context)
    {
        _context = context;
    }

    public async Task<TEntity> FirstAsync(int id)
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);
        return entity ?? throw new Exception("Entity not found");
    }

    public async Task<IEnumerable<TEntity>> ToListAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().AddRange(entities);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().UpdateRange(entities);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().RemoveRange(entities);
        await _context.SaveChangesAsync();
    }
}