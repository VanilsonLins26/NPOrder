using NP_Encomendas_BackEnd.DTOs.Request;
using NP_Encomendas_BackEnd.DTOs.Response;
using NP_Encomendas_BackEnd.Models;
using NP_Encomendas_BackEnd.Pagination;

namespace NP_Encomendas_BackEnd.Services;

public interface IProductService
{
    Task<IEnumerable<ProductResponseDto>> GetAllAsync();
    Task<ProductResponseDto> GetAsync(int id);
    Task<PagedList<ProductResponseDto>> GetPagedAsync(ProductParameters parameters);
    Task<PagedList<Product>> FilterByPricePagedAsync(ProductFilterPrice filtro);
    Task<PagedList<Product>> FilterByNamePagedAsync(ProductFilterName filtro);
    Task<ProductResponseDto> Create(ProductRequestDto dto);
    Task<ProductResponseDto> Update(int id, ProductRequestDto dto);
    Task<ProductResponseDto> Delete(int id);
}
