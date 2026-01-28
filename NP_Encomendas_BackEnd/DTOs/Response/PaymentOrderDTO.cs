using NP_Encomendas_BackEnd.Models;

namespace NP_Encomendas_BackEnd.DTOs.Response;

public class PaymentOrderDTO
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string StatusName { get; set; }
    public DateTime DeliverTime { get; set; }
}
