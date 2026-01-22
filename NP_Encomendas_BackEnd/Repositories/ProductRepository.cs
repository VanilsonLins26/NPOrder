using NP_Encomendas_BackEnd.Context;
using NP_Encomendas_BackEnd.Helpers;
using NP_Encomendas_BackEnd.Models;
using NP_Encomendas_BackEnd.Pagination;
using System.Linq.Expressions;

namespace NP_Encomendas_BackEnd.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<PagedList<Product>> GetAllPagFiltroPrecoAsync(ProductFilterPrice productFilterPrice, Expression<Func<Product, bool>>? predicate)
    {
        var finalPredicate = predicate;

        if (productFilterPrice.Price.HasValue && !string.IsNullOrEmpty(productFilterPrice.PriceFilter))
        {
            Expression<Func<Product, bool>> pricePredicate = null;

            if (productFilterPrice.PriceFilter.Equals("maior", StringComparison.OrdinalIgnoreCase))
            {
                pricePredicate = p => p.Price > productFilterPrice.Price.Value;
            }
            else if (productFilterPrice.PriceFilter.Equals("menor", StringComparison.OrdinalIgnoreCase))
            {
                pricePredicate = p => p.Price < productFilterPrice.Price.Value;
            }
            else if (productFilterPrice.PriceFilter.Equals("igual", StringComparison.OrdinalIgnoreCase))
            {
                pricePredicate = p => p.Price == productFilterPrice.Price.Value;
            }

            finalPredicate = finalPredicate.And(pricePredicate);
        }

        var products = await GetPagedAsync(
            finalPredicate, 
            p => p.Name,
            false,
            productFilterPrice.PageNumber,
            productFilterPrice.PageSize,
            p => p.Promotions
        );
        return products;
    }

    public async Task<PromotionPrice> CreatePromotion(PromotionPrice promotion)
    {
        _context.Promotions.Add(promotion); 
        return promotion;
    }



}
