﻿using Ecommerce.Application.Persistence;
using Ecommerce.Application.Specifications;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Specification;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ecommerce.Infrastructure.Persistence.Repositories;

public class RepositoryBase<T> : IAsyncRepository<T> where T : class
{

    protected readonly EcommerceDbContext _context;

    public RepositoryBase(EcommerceDbContext ecommerceDbContext)
    {
        _context = ecommerceDbContext;
    }

    public async Task<T> AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public void AddEntity(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void AddRange(List<T> entities)
    {
        _context.Set<T>().AddRange(entities);
    }


    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public void DeleteEntity(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public void DeleteRange(IReadOnlyList<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }



    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, string? includeString, bool disableTracking = true)
    {
        IQueryable<T> query = _context.Set<T>();
        if(disableTracking) query = query.AsNoTracking();

        if(!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);

        if(predicate != null) query = query.Where(predicate);

        if (orderBy != null)
            return await orderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, List<Expression<Func<T, object>>>? includes = null, bool disableTracking = true)
    {
        IQueryable<T> query = _context.Set<T>();
        if (disableTracking) query = query.AsNoTracking();

        if (includes != null) query = includes.Aggregate(query, (current, incluide) => current.Include(incluide));

        if (predicate != null) query = query.Where(predicate);

        if (orderBy != null)
            return await orderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return (await _context.Set<T>().FindAsync())!;
    }



    public async Task<T> GetEntityAsync(Expression<Func<T, bool>>? predicate, List<Expression<Func<T, object>>>? includes = null, bool disableTracking = true)
    {
        IQueryable<T> query = _context.Set<T>();
        if (disableTracking) query = query.AsNoTracking();

        if (includes != null) query = includes.Aggregate(query, (current, incluide) => current.Include(incluide));

        if (predicate != null) query = query.Where(predicate);

       
        return (await query.FirstOrDefaultAsync())!;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        
        await _context.SaveChangesAsync();

        return entity;
    }

    public void UpdateEntity(T entity)
    {

        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public async Task<T> GetByIdWithSpec(ISpecification<T> specification)
    {
        return (await ApplySpecification(specification).FirstOrDefaultAsync())!;

    }

    public async Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).ToListAsync();

    }

    public async Task<int> CountAsync(ISpecification<T> specification)
    {
        // RETORNA LA CANTIDAD
        return await ApplySpecification(specification).CountAsync();
    }

    public IQueryable<T> ApplySpecification(ISpecification<T> specification)
    {
        return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification);
    }
}
