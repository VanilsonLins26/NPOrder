using System.ComponentModel.DataAnnotations;

namespace NP_Encomendas_BackEnd.Models;

public enum DeliveryMethod
{
    [Display(Name = "Retirada na Loja")]
    Pickup = 1,

    [Display(Name = "Entrega (Delivery)")]
    Delivery = 2
}
