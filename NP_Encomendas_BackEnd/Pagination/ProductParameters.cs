namespace NP_Encomendas_BackEnd.Pagination;

public class ProductParameters : QueryStringParameters
{
    public string? name { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
