using NP_Encomendas_BackEnd.DTOs.Request;
using NP_Encomendas_BackEnd.DTOs.Response;
using NP_Encomendas_BackEnd.Models;
using NP_Encomendas_BackEnd.Pagination;
using System.Linq.Expressions;

namespace NP_Encomendas_BackEnd.Services;

public interface IProductService
{
    Task<IEnumerable<ProductResponseDto>> GetAllAsync();
    Task<ProductResponseDto> GetAsync(int id);
    Task<PagedList<ProductResponseDto>> GetPagedAsync(ProductParameters parameters, Expression<Func<Product, bool>>? predicate);
    Task<PagedList<ProductResponseDto>> FilterByPricePagedAsync(ProductFilterPrice filtro, Expression<Func<Product, bool>>? predicate);
    Task<PagedList<ProductResponseDto>> FilterByNamePagedAsync(ProductFilterName filtro, Expression<Func<Product, bool>>? predicate);
    Task<ProductResponseDto> Create(ProductRequestDto dto);
    Task<ProductResponseDto> Update(int id, ProductRequestDto dto);
    Task<ProductResponseDto> Delete(int id);
    Task<string?> UploadImage(IFormFile arquivo);
    Task<bool> SetActive(int productId, bool active);
    Task<PromotionPriceResponseDTO> SetPromotionPrice(PromotionPriceRequestDTO promotionDto);
}
