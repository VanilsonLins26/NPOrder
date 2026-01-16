using NP_Encomendas_BackEnd.Models;

namespace NP_Encomendas_BackEnd.DTOs.Request;

public class CreateOrderDTO
{
    public DateTime DeliveryTime { get; set; }
    public int? AddressId { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }

}
