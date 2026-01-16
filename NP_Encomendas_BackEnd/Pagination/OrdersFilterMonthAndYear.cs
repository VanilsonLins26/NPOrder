namespace NP_Encomendas_BackEnd.Pagination;

public class OrdersFilterMonthAndYear : QueryStringParameters
{
    public int Month { get; set; }
    public int Year { get; set; }
}
