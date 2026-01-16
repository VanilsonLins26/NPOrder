using NP_Encomendas_BackEnd.DTOs.Request;

namespace NP_Encomendas_BackEnd.DTOs.Response;

public class CartResponseDTO
{
    public CartHeaderResponseDTO CartHeader { get; set; } = new CartHeaderResponseDTO();
    public decimal Total { get; set; }
    public IEnumerable<CartItemResponseDTO> CartItems { get; set; } = Enumerable.Empty<CartItemResponseDTO>();

}
