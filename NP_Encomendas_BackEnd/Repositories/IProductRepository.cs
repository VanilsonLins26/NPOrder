using NP_Encomendas_BackEnd.Models;
using NP_Encomendas_BackEnd.Pagination;
using System.Linq.Expressions;

namespace NP_Encomendas_BackEnd.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<PromotionPrice> CreatePromotion(PromotionPrice promotion);
    Task<PagedList<Product>> GetAllPagFiltroPrecoAsync(ProductFilterPrice productFilterPrice, Expression<Func<Product, bool>>? predicate);
}
