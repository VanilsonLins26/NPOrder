using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NP_Encomendas_BackEnd.Client;
using NP_Encomendas_BackEnd.DTOs;
using NP_Encomendas_BackEnd.DTOs.MercadoPagoDTOs;
using NP_Encomendas_BackEnd.DTOs.Response;
using NP_Encomendas_BackEnd.Models;
using NP_Encomendas_BackEnd.Pagination;
using NP_Encomendas_BackEnd.Services;

namespace NP_Encomendas_BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly CreatePaymentPreferenceService _createPaymentPreferenceService;
    private readonly IOrderService _orderService;
    private readonly IPaymentService _paymentService;


    public PaymentController(CreatePaymentPreferenceService createPaymentPreferenceService, IOrderService orderService, IPaymentService paymentService)
    {
        _createPaymentPreferenceService = createPaymentPreferenceService;
        _orderService = orderService;
        _paymentService = paymentService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<PaymentResponseDTO>>> GetPayments([FromQuery] PaymentParameters parameters)
    {
        var payments = await _paymentService.GetPagedAllPayment(parameters);
        var metadata = new
        {
            payments.TotalCount,
            payments.PageSize,
            payments.CurrentPage,
            payments.TotalPages,
            payments.HasNext,
            payments.HasPrevious
        };
        Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(metadata));
        return Ok(payments);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PaymentResponseDTO>> GetById(int id)
    {
        var payment = await _paymentService.GetPaymentNoTracking(id);
        if (payment is null)
            return null;

        return Ok(payment);
    }




    [HttpPost]
    public async Task<ActionResult<CreateResponseDTO>> CreatePreference([FromBody] CheckoutDTO dto)
    {
        try
        {
            var order = await _orderService.GetOrderById(dto.OrderId);
            if (order is null)
                return BadRequest("Pedido não encontrado");

           if (order.Status != Status.PendingPayment)
                return BadRequest("Não é possível realizar pagamento para esse serviço");

            var items = new List<ItemDTO>();
            foreach(var item in order.OrderItens)
            {
                items.Add(new ItemDTO
                {
                    Id = item.Id.ToString(),
                    Quantity = item.Quantity,
                    Title = item.ProductName,
                    UnitPrice = item.UnityPrice
                });
            }

            var payer = new PayerDTO
            {
                Name = order.UserName,

            };


            var request = new CreatePreferenceRequestDTO
            {
                Items = items,
                OrderId = dto.OrderId,
                Payer = payer,
                TotalAmount = order.TotalAmount,   
                PercentToPay = dto.PercentToPay
            };
            CreateResponseDTO response = await _createPaymentPreferenceService.createPreference(request);

            return Ok(response);

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro interno");
        }
    }
}
