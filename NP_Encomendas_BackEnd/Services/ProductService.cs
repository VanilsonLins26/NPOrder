using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NP_Encomendas_BackEnd.DTOs.Request;
using NP_Encomendas_BackEnd.DTOs.Response;
using NP_Encomendas_BackEnd.Models;
using NP_Encomendas_BackEnd.Pagination;
using NP_Encomendas_BackEnd.Repositories;

namespace NP_Encomendas_BackEnd.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork uof, IMapper mapper)
    {

        _uof = uof;
        _mapper = mapper;
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

    public async Task<PagedList<Product>> FilterByNamePagedAsync(ProductFilterName filtro)
    {
        return await _uof.ProductRepository.GetPagedAsync(p => p.Name.Contains(filtro.Name), p => p.Name, false, filtro.PageNumber, filtro.PageSize);
    }

    public async Task<PagedList<Product>> FilterByPricePagedAsync(ProductFilterPrice filtro)
    {
        return await _uof.ProductRepository.GetAllPagFiltroPrecoAsync(filtro);
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
    {
        var products = await _uof.ProductRepository.GetAll().ToListAsync();

        return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
    }

    public async Task<ProductResponseDto> GetAsync(int id)
    {
        var product = await _uof.ProductRepository.GetAsync(p => p.Id == id);
        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task<PagedList<ProductResponseDto>> GetPagedAsync(ProductParameters parameters)
    {
        var products = await _uof.ProductRepository.GetPagedAsync(null, p => p.Name, false, parameters.PageNumber, parameters.PageSize);
        var productsDto = _mapper.Map<PagedList<ProductResponseDto>>(products);

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


}