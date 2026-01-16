using NP_Encomendas_BackEnd.DTOs.Response;

namespace NP_Encomendas_BackEnd.DTOs.Request
{
    public class CartRequestDTO
    {
        public CartHeaderRequestDTO CartHeader { get; set; } = new CartHeaderRequestDTO();
        public IEnumerable<CartItemRequestDTO> CartItems { get; set; } = Enumerable.Empty<CartItemRequestDTO>();
    }
}
