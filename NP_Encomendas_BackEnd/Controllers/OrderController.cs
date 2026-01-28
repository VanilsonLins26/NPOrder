using NP_Encomendas_BackEnd.DTOs.Request;
using NP_Encomendas_BackEnd.DTOs.Response;
using NP_Encomendas_BackEnd.Pagination;
using NP_Encomendas_BackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AutoMapper;

namespace NP_Encomendas_BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _Service;
    private readonly IAddressService _addressService;
    private readonly ILogger<OrderController> _logger;
    private readonly IMapper _mapper;


    public OrderController(IOrderService service, ILogger<OrderController> logger, WhatsAppService whatsAppService, IAddressService addressService, IMapper mapper)
    {
        _Service = service;
        _logger = logger;
        _addressService = addressService;
        _mapper = mapper;
    }



    [HttpGet("admin/paged")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetAllOrdersPaged([FromQuery] OrderParameters parameters)
    {

        var ordersPaged = await _Service.GetAllOrdersPaged(parameters);

        var metadata = new
        {
            ordersPaged.TotalCount,
            ordersPaged.PageSize,
            ordersPaged.CurrentPage,
            ordersPaged.TotalPages,
            ordersPaged.HasNext,
            ordersPaged.HasPrevious
        };

        Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(metadata));

        var ordersDto = _mapper.Map<IEnumerable<OrderResponseDTO>>(ordersPaged);
        return Ok(ordersDto);


    }

    [HttpGet("client/paged")]
    [Authorize(Roles = "Client")]
    public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetAllOrdersByUser([FromQuery] OrderParameters parameters)
    {
        var userId = GetUserId();
        if (userId == null)
            return BadRequest("Usuário não encontrado");

        var ordersPaged = await _Service.GetAllOrdersByUser(userId, parameters);

        var metadata = new
        {
            ordersPaged.TotalCount,
            ordersPaged.PageSize,
            ordersPaged.CurrentPage,
            ordersPaged.TotalPages,
            ordersPaged.HasNext,
            ordersPaged.HasPrevious
        };

        Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(metadata));

        var ordersDto = _mapper.Map<IEnumerable<OrderResponseDTO>>(ordersPaged);
        return Ok(ordersDto);


    }

    [Authorize(Roles = "Client, Admin")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderResponseDTO>> GetOrderById(int id)
    {

        var userId = GetUserId();

        var order = await _Service.GetOrderById(id);
        if (order == null)
            return NotFound("Encomenda não encontrada");



        return Ok(order);
    }


    [Authorize(Roles = "Client")]
    [HttpPost]
    public async Task<ActionResult<OrderResponseDTO>> CreateOrder([FromBody] CreateOrderDTO dto)
    {
        var userId = GetUserId();
        var userName = User.FindFirstValue("name");
        var phone = User.FindFirstValue("phone_number");


        if (dto.DeliveryMethod == Models.DeliveryMethod.Delivery)
        {
            if (!dto.AddressId.HasValue || dto.AddressId == 0)
                return BadRequest("Você precisa selecionar um endereço para a entrega");

            var addres = await _addressService.GetAddressById(dto.AddressId.Value, userId);

            if (addres is null)
                return BadRequest("Insira um endereço válido");
        }


        var userInfo = new UserInfoDTO
        {
            UserId = userId,
            Name = userName,
            Phone = phone
        };

        var createdOrder = await _Service.CreateOrder(dto, userInfo);
        if (createdOrder == null)
            return BadRequest("Adicione itens ao carrinho antes de solicitar uma encomenda!");

        return Ok(createdOrder);
    }


    [Authorize(Roles = "Admin")]
    [HttpPost("{orderId:int}/readyforpickup")]
    public async Task<ActionResult<OrderResponseDTO>> ReadyForPickup(int orderId)
    {
        var userId = GetUserId();
        var order = await _Service.OrderPermisson(orderId, userId);

        if (order is null)
            return BadRequest("Você não tem permissões sobre essa encomenda, ou não existe");

        var updatedOrder = await _Service.ReadyForPickup(order);

        if (updatedOrder is null)
            return BadRequest("Você só pode mudar, se estiver em 'Pedido confirmado'!");

        return Ok(updatedOrder);

    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{orderId:int}/outfordelivery")]
    public async Task<ActionResult<OrderResponseDTO>> OutForDelivery(int orderId)
    {
        var userId = GetUserId();
        var order = await _Service.OrderPermisson(orderId, userId);

        if (order is null)
            return BadRequest("Você não tem permissões sobre essa encomenda, ou não existe");

        var updatedOrder = await _Service.OutForDelivery(order);

        if (updatedOrder is null)
            return BadRequest("Você só pode mudar , se estiver em 'Pronto para retirada'!");

        return Ok(updatedOrder);

    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{orderId:int}/delivered")]
    public async Task<ActionResult<OrderResponseDTO>> Delivered(int orderId)
    {
        var userId = GetUserId();
        var order = await _Service.OrderPermisson(orderId, userId);

        if (order is null)
            return BadRequest("Você não tem permissões sobre essa encomenda, ou não existe");

        var updatedOrder = await _Service.Delivered(order);

        if (updatedOrder is null)
            return BadRequest("Você só pode mudar , se estiver em 'Saiu para entrega'!");

        return Ok(updatedOrder);

    }

    [Authorize(Roles = "Client, Admin")]
    [HttpPost("{orderId:int}/cancel")]
    public async Task<ActionResult<OrderResponseDTO>> CancelOrder(int orderId)
    {
        var userId = GetUserId();
        var order = await _Service.OrderPermisson(orderId, userId);

        if (order is null)
            return BadRequest("Você não tem permissões sobre essa encomenda, ou não existe");

        var updatedOrder = await _Service.CancelOrder(order);

        if (updatedOrder is null)
            return BadRequest("Você não pode mais cancelar esse pedido!");

        return Ok(updatedOrder);

    }

    [HttpGet("report")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ReportOrdersResponseDTO>> GetOrderReport([FromQuery] OrdersFilterMonthAndYear parameters)
    {
        var userId = GetUserId();

        var report = await _Service.GetReportByMonth(parameters, userId);

        return Ok(report);
    }

    [HttpGet("dashboard-stats")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<DashboardStatsResponseDTO>> GetDashboardStats()
    {
        var stats = await _Service.GetDashboardStats();
        return Ok(stats);
    }

    private string? GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }


}
