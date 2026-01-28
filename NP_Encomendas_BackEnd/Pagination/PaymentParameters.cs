namespace NP_Encomendas_BackEnd.Pagination;

public class PaymentParameters : QueryStringParameters
{
    public string? Status { get; set; }
    public string? FilterText { get; set; }
    public DateTime? InitialDate { get; set; }
    public DateTime? EndDate { get; set; }
}
