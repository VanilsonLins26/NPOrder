namespace NP_Encomendas_BackEnd.DTOs.Response;

public class ProductResponseDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public decimal? FinalPrice { get; set; }
    public bool IsOnSale { get; set; }
    public DateTime? PromotionEndTime { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool Active { get; set; } = true;
    public string? UnitOfMeasure { get; set; }
    public bool Customizable { get; set; }
    public DateTime CreateTime { get; set; }
}
