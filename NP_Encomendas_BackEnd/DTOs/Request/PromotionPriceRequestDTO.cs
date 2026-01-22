namespace NP_Encomendas_BackEnd.DTOs.Request;

public class PromotionPriceRequestDTO
{
    public int ProductId { get; set; }
    public decimal PromotionalPrice { get; set; }
    public DateTime InitialDate { get; set; }
    public DateTime FinalDate { get; set; }
}
