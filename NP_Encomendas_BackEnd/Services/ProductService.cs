using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NP_Encomendas_BackEnd.DTOs.Request;
using NP_Encomendas_BackEnd.DTOs.Response;
using NP_Encomendas_BackEnd.Helpers;
using NP_Encomendas_BackEnd.Models;
using NP_Encomendas_BackEnd.Pagination;
using NP_Encomendas_BackEnd.Repositories;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace NP_Encomendas_BackEnd.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProductService(IUnitOfWork uof, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {

        _uof = uof;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ProductResponseDto> Create(ProductRequestDto dto)
    {
        var product = _mapper.Map<Product>(dto);
        product.CreateTime = DateTime.Now;
        var createdProduct = _uof.ProductRepository.Create(product);
        await _uof.CommitAsync();
        return _mapper.Map<ProductResponseDto>(product);

    }

    public async Task<ProductResponseDto> Delete(int id)
    {
        var produto = await _uof.ProductRepository.GetAsync(p => p.Id == id);
        if (produto is null)
            return null;

        _uof.ProductRepository.Delete(produto);
        await _uof.CommitAsync();

        return _mapper.Map<ProductResponseDto>(produto);
    }

    public async Task<PagedList<ProductResponseDto>> FilterByNamePagedAsync(ProductFilterName filtro, Expression<Func<Product, bool>>? predicate)
    {
        predicate = predicate.And(p => p.Name.Contains(filtro.Name));
        var products = await _uof.ProductRepository.GetPagedAsync(predicate, p => p.Name, false, filtro.PageNumber, filtro.PageSize, p => p.Promotions);
        var productsDto = VerifyPromotion(products);

        var pagedResult = new PagedList<ProductResponseDto>(
        productsDto.ToList(),
        products.TotalCount,
        products.CurrentPage,
        products.PageSize
    );


        return pagedResult;
    }

    public async Task<PagedList<ProductResponseDto>> FilterByPricePagedAsync(ProductFilterPrice filtro, Expression<Func<Product, bool>>? predicate)
    {
        var products = await _uof.ProductRepository.GetAllPagFiltroPrecoAsync(filtro, predicate);
        var productsDto = VerifyPromotion(products);

        var pagedResult = new PagedList<ProductResponseDto>(
        productsDto.ToList(),
        products.TotalCount,
        products.CurrentPage,
        products.PageSize
    );


        return pagedResult;
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
    {
        var products = await _uof.ProductRepository.GetAll().ToListAsync();

        return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
    }

    public async Task<ProductResponseDto> GetAsync(int id)
    {
        var product = await _uof.ProductRepository.GetAsync(p => p.Id == id, p => p.Promotions);

        if (product == null)
        {
            return null;
        }
        var activePromo = product.Promotions?.Where(p => p.InitialDate <= DateTime.UtcNow && p.FinalDate >= DateTime.UtcNow).
                                             OrderBy(p => p.PromotionalPrice).FirstOrDefault();
        var productDto =  _mapper.Map<ProductResponseDto>(product);
        productDto.FinalPrice = activePromo != null ? activePromo.PromotionalPrice : product.Price;
        productDto.IsOnSale = activePromo != null;
        productDto.PromotionEndTime = activePromo != null ? activePromo.FinalDate: null;


        return productDto;

    }

    public async Task<PagedList<ProductResponseDto>> GetPagedAsync(ProductParameters parameters, Expression<Func<Product, bool>>? predicate)
    {
        if (parameters.name != null)
            predicate = predicate.And(p => p.Name.ToLower().Contains(parameters.name.ToLower().Trim()));

        if (parameters.MinPrice != null)
            predicate = predicate.And(p => p.Price >= parameters.MinPrice);

        if (parameters.MaxPrice != null)
            predicate = predicate.And(p => p.Price <= parameters.MaxPrice);

        var products = await _uof.ProductRepository.GetPagedAsync(predicate, p => p.Name, false, parameters.PageNumber, parameters.PageSize, p => p.Promotions);
        var productsDto = VerifyPromotion(products);

        var pagedResult = new PagedList<ProductResponseDto>(
        productsDto.ToList(),
        products.TotalCount,
        products.CurrentPage,
        products.PageSize
    );


        return pagedResult;
    }

    public async Task<ProductResponseDto> Update(int id, ProductRequestDto dto)
    {
        var product = await _uof.ProductRepository.GetAsync(p => p.Id == id);
        if (product == null)
            return null;

        _mapper.Map(dto, product);
        var productUpdated = _uof.ProductRepository.Update(product);
        await _uof.CommitAsync();
        return _mapper.Map<ProductResponseDto>(productUpdated);
    }

    public async Task<string?> UploadImage(IFormFile arquivo)
    {
        if (arquivo == null || arquivo.Length == 0) return null;

        
        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(arquivo.FileName)}";

        
        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

       
        if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

        var filePath = Path.Combine(uploadPath, fileName);

        
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await arquivo.CopyToAsync(stream);
        }

        var request = _httpContextAccessor.HttpContext?.Request;
        var baseUrl = $"{request?.Scheme}://{request?.Host}";

        return $"{baseUrl}/images/{fileName}";
    }

    public async Task<bool> SetActive(int productId, bool active)
    {
        var product = await _uof.ProductRepository.GetAsync(p => p.Id == productId);
        if (product is null)
            return false;

        product.Active = active;
        var productUpdated = _uof.ProductRepository.Update(product);
        await _uof.CommitAsync();

        if(productUpdated != null)
            return true;

        return false;

    }

    public async Task<PromotionPriceResponseDTO> SetPromotionPrice(PromotionPriceRequestDTO promotionDto)
    {
        var promotion = _mapper.Map<PromotionPrice>(promotionDto);

        var promotionCreated = await _uof.ProductRepository.CreatePromotion(promotion);
        await _uof.CommitAsync();
        return _mapper.Map<PromotionPriceResponseDTO>(promotionCreated);
    }

    private PagedList<ProductResponseDto> VerifyPromotion(PagedList<Product> products)
    {
        var productsDto = _mapper.Map<PagedList<ProductResponseDto>>(products);


        for (int i = 0; i < products.Count; i++)
        {
            var activePromo = products[i].Promotions.Where(p => p.InitialDate <= DateTime.UtcNow && p.FinalDate >= DateTime.UtcNow).
                                             OrderBy(p => p.PromotionalPrice).FirstOrDefault();
            productsDto[i].FinalPrice = activePromo != null ? activePromo.PromotionalPrice : products[i].Price;
            productsDto[i].IsOnSale = activePromo != null;
            productsDto[i].PromotionEndTime = activePromo != null ? activePromo.FinalDate: null;
        }

        return productsDto;
    }

  
}