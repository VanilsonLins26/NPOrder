namespace NP_Encomendas_BackEnd.DTOs.Request;

public class AddProductToCartDTO
{
    public int productId { get; set; }
    public int Quantity { get; set; } = 1;
    public string? Comment { get; set; }
}
