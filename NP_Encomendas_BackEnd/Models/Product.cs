namespace NP_Encomendas_BackEnd.Models;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public IEnumerable<PromotionPrice>? Promotions { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool Active { get; set; } = true;
    public string? UnitOfMeasure { get; set; }
    public bool Customizable { get; set; }
    public DateTime CreateTime { get; set; }
}
