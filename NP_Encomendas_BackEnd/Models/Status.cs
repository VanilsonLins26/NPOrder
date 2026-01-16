using System.ComponentModel.DataAnnotations;

namespace NP_Encomendas_BackEnd.Models;

public enum Status
{
    [Display(Name = "Aguardando Pagamento")]
    PendingPayment = 0,

    [Display(Name = "Pedido Confirmado")]
    Confirmed = 2,

    [Display(Name = "Pronto para Retirada")]
    ReadyForPickup = 4,

    [Display(Name = "Saiu para Entrega")]
    OutForDelivery = 5,

    [Display(Name = "Entregue")]
    Delivered = 6,

    [Display(Name = "Cancelado")]
    Canceled = 7
}
