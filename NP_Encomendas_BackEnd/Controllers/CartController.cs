using NP_Encomendas_BackEnd.DTOs.Request;
using NP_Encomendas_BackEnd.DTOs.Response;
using NP_Encomendas_BackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace NP_Encomendas_BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartService _service;

    public CartController(ICartService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Roles = "Client")]
    public async Task<ActionResult<CartResponseDTO>> GetCart()
    {
        string? userId = getUserId();
        var cart = await _service.GetCartByUserIdAsync(userId);
        return Ok(cart);
    }



    [HttpPost]
    [Authorize(Roles = "Client")]
    public async Task<ActionResult<CartResponseDTO>> AddProductToCart([FromBody] AddProductToCartDTO addProductToCartDto)
    {
        string userId = getUserId();
      
        var cart = await _service.AddProductToCart(addProductToCartDto.productId,addProductToCartDto.Quantity, userId, addProductToCartDto.Comment);
        if (cart is null)
        {
            return BadRequest("O produto não existe!");
        }
        return Ok(cart);

    }

    [HttpPost("clean/add")]
    [Authorize(Roles = "Client")]
    public async Task<ActionResult<CartResponseDTO>> CleanAndAddProductToCart([FromBody] AddProductToCartDTO addProductToCartDto)
    {
        string userId = getUserId();

        await _service.CleanCart(userId);

        var cart = await _service.AddProductToCart(addProductToCartDto.productId, addProductToCartDto.Quantity, userId, addProductToCartDto.Comment);
        if (cart is null)
        {
            return BadRequest("O produto não existe!");
        }
        return Ok(cart);

    }

        [HttpPut]
    [Authorize(Roles = "Client")]
    public async Task<ActionResult<CartResponseDTO>> UpdateProductQuantity([FromBody] ChangeProductQuantityDTO changeProductQuantityDTO)
    {
        var userId = getUserId();
        var cart = await _service.ChangeQuantityProductFromCart(changeProductQuantityDTO.CartItemId, changeProductQuantityDTO.NewQuantity, userId);
        return Ok(cart);
    }

    [HttpDelete]
    [Authorize(Roles = "Client")]
    public async Task<ActionResult<CartResponseDTO>> CleanCart()
    {
        var userId = getUserId();
        var cart = await _service.CleanCart(userId);
        return Ok(cart);
    }

    [HttpDelete("{productId:int}")]
    [Authorize(Roles = "Client")]
    public async Task<ActionResult<CartResponseDTO>> RemoveItemFromCart(int productId)
    {
        var userId = getUserId();
        var cart = await _service.DeleteProductFromCart(userId, productId);
        return Ok(cart);
    }

    private string? getUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

}
