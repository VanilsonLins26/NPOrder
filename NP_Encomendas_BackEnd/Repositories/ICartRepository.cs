using NP_Encomendas_BackEnd.DTOs.Request;
using NP_Encomendas_BackEnd.Models;

namespace NP_Encomendas_BackEnd.Repositories;

public interface ICartRepository
{
    Task<CartHeader> FindCartHeaderByUserId(string userId);
    IEnumerable<CartItem> FindCartItensByCartHeaderId(int cartHeaderId);
    bool DeleteCartItems(int cartHeaderId);
    Task<CartItem> FindCartItemById(int id);
    Task<int> CountCartItens(int cartHeaderId);
    void RemoveCartItem(CartItem cartItem);
    Task<CartHeader> FindCartHeaderById(int id);
    void RemoveCartHeader(CartHeader cartHeader);
    Task AddCartHeader(CartHeader cartHeader);
    Task<CartItem> AddCartItens(CartItem cartItem);
    Task<CartItem> FindCartItemByCartHeaderId(CartRequestDTO cartDTO, int cartHeaderId, CartHeader? cartHeader);
    void UpdateCartItens(CartItem cartItem);
    Task<CartItem> FindCartItemByProductIdAsync(int cartHeaderId, int productId);
}
