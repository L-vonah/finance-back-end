using Finance.Exceptions;
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
        if (entity != null)
        {
            return entity;
        }
        throw new EntityNotFoundException(typeof(TEntity), id);
    }

    public async Task<IEnumerable<TEntity>> ToListAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new EntityNotNullException(typeof(TEntity));
        }

        _context.Set<TEntity>().Add(entity);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            throw new FinanceException("It was not possible to add the entity.", e);
        }
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null)
        {
            throw new EntityNotNullException(typeof(TEntity));
        }

        _context.Set<TEntity>().AddRange(entities);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            throw new FinanceException("It was not possible to add the entities.", e);
        }
    }

    public async Task UpdateAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new EntityNotNullException(typeof(TEntity));
        }

        _context.Set<TEntity>().Update(entity);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            throw new FinanceException("It was not possible to update the entity.", e);
        }
    }

    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null)
        {
            throw new EntityNotNullException(typeof(TEntity));
        }

        _context.Set<TEntity>().UpdateRange(entities);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            throw new FinanceException("It was not possible to update the entities.", e);
        }
    }

    public async Task RemoveAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new EntityNotNullException(typeof(TEntity));
        }

        _context.Set<TEntity>().Remove(entity);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            throw new FinanceException("It was not possible to remove the entity.", e);
        }
    }

    public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null)
        {
            throw new EntityNotNullException(typeof(TEntity));
        }

        _context.Set<TEntity>().RemoveRange(entities);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            throw new FinanceException("It was not possible to remove the entities.", e);
        }
    }
}