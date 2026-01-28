using NP_Encomendas_BackEnd.Models;

namespace NP_Encomendas_BackEnd.Pagination;

public class OrderParameters : QueryStringParameters
{

    public OrderViewModel? ViewModel { get; set; } = OrderViewModel.Active;
    public string? FilterText { get; set; }
    public Status? StatusOptions{ get; set; }
    public DateTime? InicialDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public enum OrderViewModel
{
    
    Active = 1,
    History = 2,
    PedingPayment = 3,
}
