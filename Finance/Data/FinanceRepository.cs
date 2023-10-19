using Finance.Exceptions;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Finance.Data;

public class FinanceRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly FinanceContext _context;

    public FinanceRepository(FinanceContext context)
    {
        _context = context;
    }

    public virtual async Task<TEntity> FindAsync(int id)
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);
        if (entity != null)
        {
            return entity;
        }
        throw new EntityNotFoundException(typeof(TEntity), id);
    }

    public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> match)
    {
        var entity = await _context.Set<TEntity>().Where(match).SingleOrDefaultAsync();
        if (entity != null)
        {
            return entity;
        }
        throw new EntityNotFoundException(typeof(TEntity));
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> match)
    {
        return await _context.Set<TEntity>().Where(match).ToListAsync();
    }

    public virtual async Task AddAsync(TEntity entity)
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

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
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

    public virtual async Task UpdateAsync(TEntity entity)
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

    public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
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

    public virtual async Task RemoveAsync(TEntity entity)
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

    public virtual async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
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

    public virtual async Task<int> CountAsync()
    {
        return await _context.Set<TEntity>().CountAsync();
    }

    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _context.Set<TEntity>().Where(predicate).CountAsync();
    }

    public virtual IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
    {
        return _context.Set<TEntity>().Where(predicate);
    }

    public virtual IQueryable<TEntity> WhereIf(bool condition, Expression<Func<TEntity, bool>> predicate)
    {
        return condition ? _context.Set<TEntity>().Where(predicate) : _context.Set<TEntity>();
    }
}