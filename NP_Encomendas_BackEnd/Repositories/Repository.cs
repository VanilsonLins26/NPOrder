using Microsoft.EntityFrameworkCore;
using NP_Encomendas_BackEnd.Context;
using NP_Encomendas_BackEnd.Pagination;
using System.Linq.Expressions;

namespace NP_Encomendas_BackEnd.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public T Create(T entity)
    {
        _context.Set<T>().Add(entity);
        return entity;
    }

    public T Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        return entity;
    }

    public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
    {
        var query = _context.Set<T>().AsNoTracking().AsQueryable();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return query;
    }
    public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
        var query = _context.Set<T>().AsQueryable();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return (await query.FirstOrDefaultAsync(predicate))!;
    }


    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        return entity;
    }

    public async Task<PagedList<T>> GetPagedAsync<TKey>(Expression<Func<T, bool>>? predicate, Expression<Func<T, TKey>>? orderPredicate, bool descending, int pageNumber, int pageSize, params Expression<Func<T, object>>[] includes)
    {
        var query = _context.Set<T>().AsNoTracking().AsQueryable();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        if (orderPredicate != null)
        {
            if (descending)
                query = query.OrderByDescending(orderPredicate);

            else
                query = query.OrderBy(orderPredicate);
        }
        return await PagedList<T>.ToPagedListAsync(query, pageNumber, pageSize);
    }


}
