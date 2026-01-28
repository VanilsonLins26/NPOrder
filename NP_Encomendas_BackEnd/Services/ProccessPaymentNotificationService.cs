using AutoMapper;
using MercadoPago.Client;
using NP_Encomendas_BackEnd.Client;
using NP_Encomendas_BackEnd.DTOs.MercadoPagoDTOs;
using NP_Encomendas_BackEnd.DTOs.Request;
using NP_Encomendas_BackEnd.Models;
using System.Globalization;

namespace NP_Encomendas_BackEnd.Services;

public class ProccessPaymentNotificationService
{
    private readonly MercadoPagoService MercadoPagoService;
    private readonly IOrderService _orderService;
    private readonly ILogger<ProccessPaymentNotificationService> _logger;
    private readonly WhatsAppService _whatsAppService;
    private readonly IPaymentService _paymentService;
    private readonly IMapper _mapper;

    public ProccessPaymentNotificationService(MercadoPagoService mercadoPagoService, IOrderService orderService, ILogger<ProccessPaymentNotificationService> logger, WhatsAppService whatsAppService, IPaymentService paymentService, IMapper mapper)
    {
        MercadoPagoService = mercadoPagoService;
        _orderService = orderService;
        _logger = logger;
        _whatsAppService = whatsAppService;
        _paymentService = paymentService;
        _mapper = mapper;
    }

    public async Task<ProccessNotificationResponseDTO> ProccessPaymentNotification(string id, string type)
    {

        try
        {
            PaymentEntity payment = await MercadoPagoService.GetPaymentStatus(long.Parse(id));
            var localPayment = await _paymentService.GetPaymentNoTracking(payment.Id);

            var updatedPayment = _mapper.Map<PaymentRequestDTO>(payment);

            var orderId = localPayment.OrderId;
            updatedPayment.OrderId = orderId;
            updatedPayment.DateCreated = localPayment.DateCreated;

            await _paymentService.UpdatePayment(payment.Id, updatedPayment);

            if("approved".Equals(payment.Status, StringComparison.OrdinalIgnoreCase))
            {

                await _orderService.ConfirmOrder(orderId, payment.TransactionAmount ?? 0);
                await _whatsAppService.SendOrderNotificationAsync(orderId);
                await _whatsAppService.SendCustomerConfirmationAsync(orderId);
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
