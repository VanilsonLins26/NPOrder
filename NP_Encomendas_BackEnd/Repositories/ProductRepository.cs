using NP_Encomendas_BackEnd.Context;
using NP_Encomendas_BackEnd.Models;
using NP_Encomendas_BackEnd.Pagination;

namespace NP_Encomendas_BackEnd.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<PagedList<Product>> GetAllPagFiltroPrecoAsync(ProductFilterPrice productFilterPrice)
    {
        PagedList<Product> produtos = new PagedList<Product>();

        if (productFilterPrice.Price.HasValue && !string.IsNullOrEmpty(productFilterPrice.PriceFilter))
        {
            if (productFilterPrice.PriceFilter.Equals("maior", StringComparison.OrdinalIgnoreCase))
            {
                produtos = await GetPagedAsync(p => p.Price > productFilterPrice.Price.Value, p => p.Name, false, productFilterPrice.PageNumber, productFilterPrice.PageSize);
            }
            else if (productFilterPrice.PriceFilter.Equals("menor", StringComparison.OrdinalIgnoreCase))
            {
                produtos = await GetPagedAsync(p => p.Price < productFilterPrice.Price.Value, p => p.Name, false, productFilterPrice.PageNumber, productFilterPrice.PageSize);
            }
            else if (productFilterPrice.PriceFilter.Equals("igual", StringComparison.OrdinalIgnoreCase))
            {
                produtos = await GetPagedAsync(p => p.Price == productFilterPrice.Price.Value, p => p.Name, false, productFilterPrice.PageNumber, productFilterPrice.PageSize);
            }


        }
        return produtos;
    }


}
