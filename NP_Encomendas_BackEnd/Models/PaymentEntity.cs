namespace NP_Encomendas_BackEnd.Models;

public class PaymentEntity
{
    public string Id { get; set; }
    public string Status { get; set; }
    public string OrderId { get; set; }
    public string Amount { get; set; }
    public string StatusDetail { get; set; }
    public string PaymentMethodId { get; set; }
    public Payer Payer { get; set; }
}
