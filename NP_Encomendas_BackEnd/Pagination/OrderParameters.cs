namespace NP_Encomendas_BackEnd.Pagination;

public class OrderParameters : QueryStringParameters
{

    public OrderViewModel ViewModel { get; set; } = OrderViewModel.All;
}

public enum OrderViewModel
{
    All = 0,
    Active = 1,
    History = 2
}
