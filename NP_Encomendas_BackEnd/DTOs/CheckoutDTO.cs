using System.ComponentModel.DataAnnotations;

namespace NP_Encomendas_BackEnd.DTOs;

public class CheckoutDTO
{
    public int OrderId { get; set; }
    [AllowedValues(50, 100, ErrorMessage = "A porcentagem deve ser 50 ou 100.")]
    public int PercentToPay { get; set; }
}
