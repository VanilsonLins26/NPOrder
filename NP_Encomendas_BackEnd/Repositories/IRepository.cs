using NP_Encomendas_BackEnd.Pagination;
using System.Linq.Expressions;

namespace NP_Encomendas_BackEnd.Repositories;

public interface IRepository<T>
{
    IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
    Task<PagedList<T>> GetPagedAsync<TKey>(Expression<Func<T, bool>>? predicate, Expression<Func<T, TKey>>? orderPredicate, bool descending, int pageNumber, int pageSize, params Expression<Func<T, object>>[] includes);
    Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

    T Create(T entity);
    T Update(T entity);
    T Delete(T entity);


}
