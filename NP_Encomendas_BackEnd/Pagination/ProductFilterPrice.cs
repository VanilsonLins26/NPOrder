namespace NP_Encomendas_BackEnd.Pagination;

public class ProductFilterPrice : QueryStringParameters
{
    public decimal? Price { get; set; }
    public string? PriceFilter { get; set; }
}
