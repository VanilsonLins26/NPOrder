namespace NP_Encomendas_BackEnd.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal UnityPrice { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal TotalPrice => UnityPrice * Quantity;
    public string? Comment { get; set; }
    public Product Product { get; set; }
}
