namespace NP_Encomendas_BackEnd.DTOs.Request;

public class ChangeProductQuantityDTO
{
    public int CartItemId { get; set; }
    public int NewQuantity { get; set; }
}
