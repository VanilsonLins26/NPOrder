using NP_Encomendas_BackEnd.Models;

namespace NP_Encomendas_BackEnd.Models;

public class CartItem
{
    public int Id { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal UnityPrice { get; set; }
    public int ProductId { get; set; }
    public int CartHeaderId { get; set; }
    public decimal TotalPrice { get { return (UnityPrice * Quantity); } }
    public string? Comment { get; set; }
    public Product Product { get; set; }

}
