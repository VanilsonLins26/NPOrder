using NP_Encomendas_BackEnd.Models;

namespace NP_Encomendas_BackEnd.DTOs.Request;

public class OrderRequestDTO
{
    public string ClientId { get; set; }
    public int? AddressId { get; set; }
    public string UserName { get; set; }
    public string Phone { get; set; }
    public Status Status { get; set; }
    public ICollection<OrderItemRequestDTO> OrderItens { get; set; }
    public DateTime DeliverTime { get; set; }
    public decimal AmountPaid { get; set; }
  
}
