using NP_Encomendas_BackEnd.Context;
using NP_Encomendas_BackEnd.DTOs.Request;
using NP_Encomendas_BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace NP_Encomendas_BackEnd.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;
    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CartHeader> FindCartHeaderByUserId(string userId)
    {
        var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(ch => ch.UserId == userId);
        return cartHeader;

    }

    public IEnumerable<CartItem> FindCartItensByCartHeaderId(int cartHeaderId)
    {
        var cartItems = _context.CartItems
                            .Where(c => c.CartHeaderId == cartHeaderId)
                            .Include(c => c.Product);
        return cartItems;
    }

    public async Task<CartItem> FindCartItemByCartHeaderId(CartRequestDTO cartDTO, int cartHeaderId, CartHeader? cartHeader)
    {
        var cartItem = await _context.CartItems.AsNoTracking().FirstOrDefaultAsync(
                                 p => p.ProductId == cartDTO.CartItems.FirstOrDefault()
                                 .ProductId && p.CartHeaderId == cartHeader.Id);
        return cartItem;
    }

    public async Task<CartItem> FindCartItemByProductIdAsync(int cartHeaderId, int productId)
    {
        return await _context.CartItems
                             .FirstOrDefaultAsync(ci => ci.CartHeaderId == cartHeaderId &&
                                                         ci.ProductId == productId);

    }



    public async Task<CartItem> FindCartItemById(int id)
    {
        return await _context.CartItems.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<CartHeader> FindCartHeaderById(int id)
    {
        return await _context.CartHeaders.FirstOrDefaultAsync(c => c.Id == id);
    }

    public bool DeleteCartItems(int cartHeaderId)
    {
        var itens = _context.CartItems.Where(c => c.CartHeaderId == cartHeaderId);

        if (!itens.Any())
            return false;

        _context.CartItems.RemoveRange(itens);

        return true;

    }

    public void RemoveCartItem(CartItem cartItem)
    {
        _context.CartItems.Remove(cartItem);
    }

    public void RemoveCartHeader(CartHeader cartHeader)
    {
        _context.CartHeaders.Remove(cartHeader);
    }

    public async Task<int> CountCartItens(int cartHeaderId)
    {
        return await _context.CartItems.Where(c => c.CartHeaderId == cartHeaderId).CountAsync();
    }

    public async Task AddCartHeader(CartHeader cartHeader)
    {
        await _context.CartHeaders.AddAsync(cartHeader);
    }

    public async Task<CartItem> AddCartItens(CartItem cartItem)
    {
        await _context.CartItems.AddAsync(cartItem);
        return cartItem;
    }

    public void UpdateCartItens(CartItem cartItem)
    {
        _context.CartItems.Update(cartItem);
    }

}
