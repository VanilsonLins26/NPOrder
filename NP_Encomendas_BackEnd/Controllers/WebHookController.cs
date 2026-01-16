using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NP_Encomendas_BackEnd.DTOs.MercadoPagoDTOs;
using NP_Encomendas_BackEnd.Services;

namespace NP_Encomendas_BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WebHookController : ControllerBase
{
    private readonly ProccessPaymentNotificationService _paymentService;

    public WebHookController(ProccessPaymentNotificationService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("mercadopago")]
    public async Task<ActionResult> HandlerNotification([FromBody] MercadoPagoConfigDTO mercadoPagoConfig)
    {
        string resourceId = mercadoPagoConfig.Data.Id;
        string resourceType = mercadoPagoConfig.Type;

        try
        {
            var result = await _paymentService.ProccessPaymentNotification(resourceId, resourceType);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Erro interno");
        }
        return Ok();

    }
}
