namespace NP_Encomendas_BackEnd.Models;

public class PromotionPrice
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public decimal PromotionalPrice { get; set; }
    public DateTime InitialDate { get; set; }
    public DateTime FinalDate { get; set; }
    public Product product { get; set; }


}
