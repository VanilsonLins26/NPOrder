using NP_Encomendas_BackEnd.DTOs;
using NP_Encomendas_BackEnd.DTOs.Request;
using NP_Encomendas_BackEnd.DTOs.Response;
using NP_Encomendas_BackEnd.Models;

namespace NP_Encomendas_BackEnd.Services;

public interface ICartService
{
    Task<CartResponseDTO> GetCartByUserIdAsync(string userId);
    Task<CartResponseDTO> AddProductToCart(int productId, int quantity, string userId, string comment);
    Task<CartResponseDTO> CleanCart(string userId);
    Task<CartResponseDTO> DeleteProductFromCart(string userId, int productId);
    Task<CartResponseDTO> ChangeQuantityProductFromCart(int cartItemId, int newQuantity, string userId);
    Task<CartResponseDTO> CreateCart(string userId);
}
