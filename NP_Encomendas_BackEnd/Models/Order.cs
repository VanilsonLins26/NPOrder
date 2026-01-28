using MercadoPago.Resource.Common;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace NP_Encomendas_BackEnd.Models;

public class Order
{
    public int Id { get; set; }
    public string ClientId { get; set; }
    public int? AddressId { get; set; }
    public string UserName { get; set; }
    public string Phone { get; set; }
    public Status Status { get; set; }
    public ICollection<OrderItem> OrderItens { get; set; } = new List<OrderItem>();
    public DeliveryMethod DeliveryMethod { get; set; }
    public Address? Address { get; set; }
    public DateTime DeliverTime { get; set; } 
    public DateTime OrderTime { get; set; } = DateTime.Now;
    public decimal TotalAmount { get; set; }
    public PaymentEntity? Payment { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal AmountPaid { get; set; }
    public decimal RemainingAmount => TotalAmount - AmountPaid;
    public bool IsFullyPaid => AmountPaid >= TotalAmount;

}
