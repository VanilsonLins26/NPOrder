using NP_Encomendas_BackEnd.DTOs;
using NP_Encomendas_BackEnd.DTOs.Request;
using NP_Encomendas_BackEnd.DTOs.Response;
using NP_Encomendas_BackEnd.Models;
using NP_Encomendas_BackEnd.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace NP_Encomendas_BackEnd.Services;

public class CartService : ICartService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uof;

    public CartService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _uof = unitOfWork;
    }

    public async Task<CartResponseDTO> GetCartByUserIdAsync(string userId)
    {
        CartHeader cartHeader = await _uof.CartRepository.FindCartHeaderByUserId(userId);

        
        if (cartHeader is null)
            return null;

        Cart cart = new() { CartHeader = cartHeader };
        cart.CartItems = _uof.CartRepository.FindCartItensByCartHeaderId(cartHeader.Id);

        return _mapper.Map<CartResponseDTO>(cart);
    }

    public async Task<CartResponseDTO> AddProductToCart(int productId, int quantity, string userId, string? comment)
    {
        var product = await _uof.ProductRepository.GetAsync(p => p.Id == productId);
        if (product is null)
            return null;

        var cart = await GetCartByUserIdAsync(userId);
        if (cart is null)
        {
            cart = await CreateCart(userId);
        }
        var carItem = await _uof.CartRepository.FindCartItemByProductIdAsync(cart.CartHeader.Id, product.Id);
        if (carItem is null || product.Customizable)
        {
            var cartItem = await _uof.CartRepository.AddCartItens(new CartItem
            {
                CartHeaderId = cart.CartHeader.Id,
                ProductId = product.Id,
                Quantity = quantity,
                UnityPrice = product.Price,
                Comment = comment
            });
        }
        else
        {
                carItem.Quantity += quantity;
            _uof.CartRepository.UpdateCartItens(carItem);
        }
        await _uof.CommitAsync();
        return await GetCartByUserIdAsync(userId);
    }

    public async Task<CartResponseDTO> CleanCart(string userId)
    {
        var cartHeader = await _uof.CartRepository.FindCartHeaderByUserId(userId);   
        var deleteCartItens =  _uof.CartRepository.DeleteCartItems(cartHeader.Id);
        await _uof.CommitAsync();

        return await GetCartByUserIdAsync(userId); ;
        
    }

    public async Task<CartResponseDTO> DeleteProductFromCart(string userId, int productId)
    {
        var cartHeader = await _uof.CartRepository.FindCartHeaderByUserId(userId);
        var cartItem = await _uof.CartRepository.FindCartItemByProductIdAsync(cartHeader.Id, productId);
        if (cartItem is not null)
        {
            _uof.CartRepository.RemoveCartItem(cartItem);
            await _uof.CommitAsync();
        }
        
        return await GetCartByUserIdAsync(userId);
    }

    public async Task<CartResponseDTO> ChangeQuantityProductFromCart(int cartItemId, int newQuantity, string userId)
    {
        var cartItem = await _uof.CartRepository.FindCartItemById(cartItemId);

        var cartHeader = await _uof.CartRepository.FindCartHeaderById(cartItem.CartHeaderId);

        if (cartHeader.UserId != userId)
            return null;

        if (newQuantity <= 0)
            _uof.CartRepository.RemoveCartItem(cartItem);

        else
        {
            cartItem.Quantity = newQuantity;
            _uof.CartRepository.UpdateCartItens(cartItem);
        }

        await _uof.CommitAsync();

        return await GetCartByUserIdAsync(userId);
    }
   

    public async Task<CartResponseDTO> CreateCart(string userId)
    {
        var cartHeader = new CartHeader
        {
            UserId = userId,
        };
        await _uof.CartRepository.AddCartHeader(cartHeader);

        await _uof.CommitAsync();

        var cart = new Cart
        {
            CartHeader = cartHeader
        };

        return _mapper.Map<CartResponseDTO>(cart);

    }

  
}
