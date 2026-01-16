using MercadoPago.Client;
using NP_Encomendas_BackEnd.Client;
using NP_Encomendas_BackEnd.DTOs.MercadoPagoDTOs;
using NP_Encomendas_BackEnd.Models;
using System.Globalization;

namespace NP_Encomendas_BackEnd.Services;

public class ProccessPaymentNotificationService
{
    private readonly MercadoPagoService MercadoPagoService;
    private readonly IOrderService _orderService;
    private readonly ILogger<ProccessPaymentNotificationService> _logger;
    private readonly WhatsAppService _whatsAppService;

    public ProccessPaymentNotificationService(MercadoPagoService mercadoPagoService, IOrderService orderService, ILogger<ProccessPaymentNotificationService> logger, WhatsAppService whatsAppService)
    {
        MercadoPagoService = mercadoPagoService;
        _orderService = orderService;
        _logger = logger;
        _whatsAppService = whatsAppService;
    }

    public async Task<ProccessNotificationResponseDTO> ProccessPaymentNotification(string id, string type)
    {

        try
        {
            PaymentEntity payment = await MercadoPagoService.GetPaymentStatus(long.Parse(id));

            var preference = payment.OrderId;
            string[] parts = preference.Split("_");
     

            var userId = long.Parse(parts[0]);

            if("approved".Equals(payment.Status, StringComparison.OrdinalIgnoreCase))
            {
                await _orderService.ConfirmOrder(int.Parse(payment.OrderId), decimal.Parse(payment.Amount,CultureInfo.InvariantCulture));
                await _whatsAppService.SendOrderNotificationAsync(payment.OrderId);
                await _whatsAppService.SendCustomerConfirmationAsync(payment.OrderId);
            }

            return new ProccessNotificationResponseDTO
            {
                Success = true,
                UpdateStatus = payment.Status
            };
        }
        catch(Exception e)
        {
            _logger.LogError("Erro ao processar pagamento {e}", e);
            return new ProccessNotificationResponseDTO
            {
                Success = false,
                UpdateStatus = "SERVER_ERROR"
            };
        }

    }
    
}
