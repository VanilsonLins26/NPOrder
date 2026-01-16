namespace NP_Encomendas_BackEnd.DTOs.MercadoPagoDTOs;

public class CreatePreferenceRequestDTO
{
    public long UserId { get; set; }
    public int OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public PayerDTO Payer { get; set; }
    public BackUrlsDTO BackUrls { get; set; }
    public SimpleAddressDTO deliveryAddress { get; set; }
    public string AutoReturn { get; set; }
    public IEnumerable<ItemDTO> Items { get; set; }
    public int PercentToPay { get; set; }

}
