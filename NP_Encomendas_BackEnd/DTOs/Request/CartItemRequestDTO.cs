using NP_Encomendas_BackEnd.DTOs.Request;

namespace NP_Encomendas_BackEnd.DTOs.Request;

public class CartItemRequestDTO
{
    public int Quantity { get; set; } = 1;
    public int ProductId { get; set; }
    public int CartHeaderId { get; set; }
    public decimal UnityPrice { get; set; }
    public ICollection<string>? ImageUrls { get; set; }
    public string? Comment { get; set; }
    public ProductRequestDto Product { get; set; } = new ProductRequestDto();
}
