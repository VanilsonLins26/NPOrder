namespace NP_Encomendas_BackEnd.DTOs.Response;

public class PromotionPriceResponseDTO
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public decimal PromotionalPrice { get; set; }
    public DateTime InitialDate { get; set; }
    public DateTime FinalDate { get; set; }
}
