using NP_Encomendas_BackEnd.Models;

namespace NP_Encomendas_BackEnd.DTOs.Response;

public class OrderResponseDTO
{
    public int Id { get; set; }
    public string ClientId { get; set; }
    public int? AddressId { get; set; }
    public Address Address { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public string UserName { get; set; }
    public string Phone { get; set; }
    public Status Status { get; set; }
    public string StatusName { get; set; }
    public IEnumerable<OrderItem> OrderItens { get; set; }
    public DateTime DeliverTime { get; set; }
    public DateTime OrderTime { get; set; }
    public decimal TotalAmount { get; set; }
    public PaymentResponseDTO? Payment { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal RemainingAmount => TotalAmount - AmountPaid;
    public bool IsFullyPaid => AmountPaid >= TotalAmount;
}
