using NP_Encomendas_BackEnd.Models;

namespace NP_Encomendas_BackEnd.DTOs.Request;

public class PaymentRequestDTO
{
    public string Status { get; set; }
    public int OrderId { get; set; }
    public decimal? TransactionAmount { get; set; }
    public decimal? NetReceivedAmount { get; set; }
    public decimal? FeeAmount { get; set; }
    public int? Installments { get; set; }
    public string? StatusDetail { get; set; }
    public string? PaymentMethodId { get; set; }
    public string? PaymentTypeId { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateApproved { get; set; }
    public DateTime? MoneyReleaseDate { get; set; }
    public string? PaymentUrl { get; set; }
}
