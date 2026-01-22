
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NP_Encomendas_BackEnd.DTOs.Request;
using NP_Encomendas_BackEnd.DTOs.Response;
using NP_Encomendas_BackEnd.Models;
using NP_Encomendas_BackEnd.Pagination;
using NP_Encomendas_BackEnd.Services;
using System.Linq.Expressions;
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
        var role = User.FindFirstValue(ClaimTypes.Role);
        Expression<Func<Product, bool>>? predicate = null;

        if (role != "Admin")
            predicate = p => p.Active == true;

        var productPaged = await _service.GetPagedAsync(parameters, predicate);

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
        var predicate = GetUserPredicate();

        var productsPaged = await _service.FilterByPricePagedAsync(parameters, predicate);
        return Ok(productsPaged);
    }

    

    [AllowAnonymous]
    [HttpGet("filter/name")]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetPagedByNameAsync([FromQuery] ProductFilterName parameters)
    {
        var predicate = GetUserPredicate();

        var productsPaged = await _service.FilterByNamePagedAsync(parameters, predicate);
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

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> CreateAsync([FromForm]ProductRequestDto productDto)
    {
        if (productDto.ImageFile != null)
        {
            productDto.ImageUrl = await _service.UploadImage(productDto.ImageFile);
        }

        var productCreated = await _service.Create(productDto);

        if (productCreated == null)
            return BadRequest();

        return Ok(productCreated);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductResponseDto>> UpdateAsync(int id, [FromForm] ProductRequestDto productDto)
    {
        if (productDto.ImageFile != null)
        {
            productDto.ImageUrl = await _service.UploadImage(productDto.ImageFile);
        }

        var product = await _service.GetAsync(id);


        var productUpdated = await _service.Update(id, productDto);


        if(productUpdated == null)
            return NotFound("Produto não encontrado");

        return Ok(productUpdated);
    }

    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "Admin")]
    [HttpPatch("active/{produtctId:int}")]
    public async Task<IActionResult> SetActive(int produtctId, [FromBody] bool active)
    {
        var result = await _service.SetActive(produtctId, active);
        if (result == false)
            return BadRequest("Ocorreu um erro!");

        return NoContent();

    }

    [Authorize(Roles = "Admin")]
    [HttpPost("promotion")]
    public async Task<ActionResult<PromotionPriceResponseDTO>> SetPromotionPrice(PromotionPriceRequestDTO promotionDto)
    {
        var promotionCreated = await _service.SetPromotionPrice(promotionDto);
        if (promotionCreated is null)
            return BadRequest("Ocorreu um erro ao adicionar a promoção");

        return Ok(promotionCreated);
    }

    private Expression<Func<Product, bool>>? GetUserPredicate()
    {
        var role = User.FindFirstValue(ClaimTypes.Role);
        Expression<Func<Product, bool>>? predicate = null;

        if (role != "Admin")
            predicate = p => p.Active == true;
        return predicate;
    }




}
