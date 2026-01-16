using NP_Encomendas_BackEnd.DTOs.Response;

namespace NP_Encomendas_BackEnd.DTOs.Response;

public class OrderItemResponseDTO
{
    public int Id { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal UnityPrice { get; set; }
    public decimal TotalPrice { get { return (UnityPrice * Quantity); } }
    public int ProductId { get; set; }
    public int OrderId { get; set; }
    public string? Comment { get; set; }
    public ProductResponseDto Product { get; set; } = new ProductResponseDto();
}
