﻿using Finance.Exceptions;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Finance.Data;

public class FinanceRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly FinanceContext _context;
    private DbSet<TEntity> _dbSet => _context.Set<TEntity>();

    public FinanceRepository(FinanceContext context)
    {
        _context = context;
    }

    public virtual async Task<TEntity> FindAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(TEntity), id);
        }
        return entity;
    }

    public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> match)
    {
        var entity = await Where(match).SingleOrDefaultAsync();
        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(TEntity));
        }
        return entity;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.TagWithCallSite().ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> match)
    {
        return await Where(match).TagWithCallSite().ToListAsync();
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new EntityNotNullException(typeof(TEntity));
        }

        _dbSet.Add(entity);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
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

        _dbSet.AddRange(entities);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
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

        _dbSet.Update(entity);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
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

        _dbSet.UpdateRange(entities);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
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

        _dbSet.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
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

        _dbSet.RemoveRange(entities);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            throw new FinanceException("It was not possible to remove the entities.", e);
        }
    }

    public virtual async Task<int> CountAsync()
    {
        return await _dbSet.CountAsync();
    }

    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Where(predicate).CountAsync();
    }

    public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.Where(predicate);
    }

    public IQueryable<TEntity> AsQueryable()
    {
        return _dbSet.AsQueryable();
    }

    private IQueryable<TEntity> WhereIf(bool condition, Expression<Func<TEntity, bool>> predicate)
    {
        return condition ? Where(predicate) : _dbSet;
    }

    public virtual async Task<IEnumerable<TResult>> SelectListAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector)
    {
        return await _dbSet.Select(selector).TagWithCallSite().ToListAsync();
    }

    public virtual async Task<IEnumerable<TResult>> SelectListAsync<TResult>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TResult>> selector)
    {
        return await Where(predicate).Select(selector).TagWithCallSite().ToListAsync();
    }

    public virtual async Task<TResult> SelectSingleAsync<TResult>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TResult>> selector)
    {
        var result = await Where(predicate).Select(selector).SingleOrDefaultAsync();
        if (result == null)
        {
            throw new EntityNotFoundException(typeof(TEntity));
        }

        return result;
    }
}