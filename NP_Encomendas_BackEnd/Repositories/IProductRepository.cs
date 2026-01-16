using NP_Encomendas_BackEnd.Models;
using NP_Encomendas_BackEnd.Pagination;

namespace NP_Encomendas_BackEnd.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<PagedList<Product>> GetAllPagFiltroPrecoAsync(ProductFilterPrice productFilterPrice);
}
