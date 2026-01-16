
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NP_Encomendas_BackEnd.DTOs.Request;
using NP_Encomendas_BackEnd.DTOs.Response;
using NP_Encomendas_BackEnd.Pagination;
using NP_Encomendas_BackEnd.Services;
using System.Security.Claims;

namespace NP_Encomendas_BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;
    private readonly IMapper _mapper;

    public ProductController(IProductService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAllAsync()
    {
        var products = await _service.GetAllAsync();

        return Ok(products);
    }

    [AllowAnonymous]
    [HttpGet("paged")]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAllPagedAsync([FromQuery]ProductParameters parameters)
    {
        var productPaged = await _service.GetPagedAsync(parameters);

        var metadata = new
        {
            productPaged.TotalCount,
            productPaged.PageSize,
            productPaged.CurrentPage,
            productPaged.TotalPages,
            productPaged.HasNext,
            productPaged.HasPrevious
        };

        Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(metadata));

        var produtosDto = _mapper.Map<IEnumerable<ProductResponseDto>>(productPaged);
        return Ok(produtosDto);
    }

    [AllowAnonymous]
    [HttpGet("filter/price")]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetPagedByPriceAsync([FromQuery] ProductFilterPrice parameters)
    {
        var productsPaged = await _service.FilterByPricePagedAsync(parameters);
        return Ok(productsPaged);
    }

    [AllowAnonymous]
    [HttpGet("filter/name")]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetPagedByNameAsync([FromQuery] ProductFilterName parameters)
    {
        var productsPaged = await _service.FilterByNamePagedAsync(parameters);
        return Ok(productsPaged);
    }

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponseDto>> GetByIdAsync(int id)
    {
        var product = await _service.GetAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    //[Authorize(Roles = "StoreAdmin")]
    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> CreateAsync([FromBody]ProductRequestDto productDto)
    {


        var productCreated = await _service.Create(productDto);

        if (productCreated == null)
            return BadRequest();

        return Ok(productCreated);
    }

    //[Authorize(Roles = "StoreAdmin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductResponseDto>> UpdateAsync(int id, [FromBody] ProductRequestDto productDto)
    {


        var product = await _service.GetAsync(id);


        var productUpdated = await _service.Update(id, productDto);


        if(productUpdated == null)
            return NotFound("Produto não encontrado");

        return Ok(productUpdated);
    }

    [Authorize(Roles = "StoreAdmin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ProductResponseDto>> DeleteAsync(int id)
    {
     

        var product = await _service.GetAsync(id);

        if(product == null)
        {
            return NotFound("Produto não encontrado");
        }

        var productDeleted = await _service.Delete(id);
        if(productDeleted == null)
            return BadRequest();

        return Ok(productDeleted);
    }

}
