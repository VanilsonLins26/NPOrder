using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.EntityFrameworkCore;
using NP_Encomendas_BackEnd.Client;
using NP_Encomendas_BackEnd.DTOs.Request;
using NP_Encomendas_BackEnd.DTOs.Response;
using NP_Encomendas_BackEnd.Helpers;
using NP_Encomendas_BackEnd.Models;
using NP_Encomendas_BackEnd.Pagination;
using NP_Encomendas_BackEnd.Repositories;
using System.Linq.Expressions;

namespace NP_Encomendas_BackEnd.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _uof;
    private readonly ICartService _cartService;
    private readonly IAddressService _addressService;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork uof, IMapper mapper, ICartService cartService, IAddressService addressService)
    {
        _uof = uof;
        _mapper = mapper;
        _cartService = cartService;
        _addressService = addressService;
    }

    public async Task<OrderResponseDTO> GetOrderByIdNoTracking(int orderId)
    {
        var order = await _uof.OrderRepository.GetOrderByIdNoTracking(orderId);
        return _mapper.Map<OrderResponseDTO>(order);
    }

    public async Task<PagedList<OrderResponseDTO>> GetAllOrdersPaged(OrderParameters parameters)
    {
        bool descending = true;
        List<Status> statusList = new List<Status>();

        if (parameters.StatusOptions.HasValue)
        {
            statusList.Add(parameters.StatusOptions.Value);
        }

        else if (parameters.ViewModel == OrderViewModel.Active)
        {
            statusList = [
            Status.Confirmed,
            Status.ReadyForPickup,
            Status.OutForDelivery
                ];
            descending = false; 
        }
        else if (parameters.ViewModel == OrderViewModel.History)
        {
            statusList = [
                Status.Delivered,
                Status.Canceled
                ];
        }
        else if (parameters.ViewModel == OrderViewModel.PedingPayment)
        {
            statusList = [
                Status.PendingPayment
                ];
        }

        Expression<Func<Order, bool>>? predicate = o => statusList.Contains(o.Status);

        if (parameters.FilterText != null)
        {
            predicate = predicate.And(o => o.UserName.Contains(parameters.FilterText) || o.Id.ToString() == parameters.FilterText);

        }

        if (parameters.InicialDate != null)
        {
            predicate = predicate.And( o => o.DeliverTime >= parameters.InicialDate);
        }
        if (parameters.EndDate != null)
        {
            predicate = predicate.And(o => o.DeliverTime <= parameters.EndDate);
        }


        var orders = await _uof.OrderRepository.GetPagedAsync(predicate, o => o.DeliverTime, descending,
                        parameters.PageNumber, parameters.PageSize, o => o.OrderItens, o => o.Address);

        var ordersDto = _mapper.Map<PagedList<OrderResponseDTO>>(orders);

        return new PagedList<OrderResponseDTO>(
        ordersDto.ToList(),
        orders.TotalCount,
        orders.CurrentPage,
        orders.PageSize
    );
    }

    public async Task<PagedList<OrderResponseDTO>> GetAllOrdersByUser(string userId, OrderParameters parameters)
    {
        var orders = await _uof.OrderRepository.GetPagedAsync(o => o.ClientId == userId, o => o.DeliverTime, true,
                           parameters.PageNumber, parameters.PageSize, o => o.OrderItens, o => o.Address);

        var ordersDto = _mapper.Map<PagedList<OrderResponseDTO>>(orders);

        return new PagedList<OrderResponseDTO>(
        ordersDto.ToList(),
        orders.TotalCount,
        orders.CurrentPage,
        orders.PageSize
    );

    }

    public async Task<OrderResponseDTO> GetOrderById(int orderId)
    {
        var order = await _uof.OrderRepository.GetAsync(o => o.Id == orderId, o => o.OrderItens, o => o.Address, o => o.Payment);
        if (order == null)
            return null;

        return _mapper.Map<OrderResponseDTO>(order);
    }


    public async Task<OrderResponseDTO> CreateOrder(CreateOrderDTO dto, UserInfoDTO userInfo)
    {

        try
        {


            var cartDTO = await _cartService.GetCartByUserIdAsync(userInfo.UserId);

            if (!cartDTO.CartItems.Any())
                return null;



            var order = new Order
            {
                ClientId = userInfo.UserId,
                DeliverTime = dto.DeliveryTime.AddHours(-3),
                Status = Status.PendingPayment,
                UserName = userInfo.Name,
                Phone = userInfo.Phone,
                AddressId = dto.DeliveryMethod == DeliveryMethod.Delivery ? dto.AddressId : null,
                DeliveryMethod = dto.DeliveryMethod,
                OrderTime = GetBrasiliaTime()

            };


            foreach (var item in cartDTO.CartItems)
            {
                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Comment = item.Comment,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    UnityPrice = item.UnityPrice

                };
                order.OrderItens.Add(orderItem);

                order.TotalAmount += orderItem.TotalPrice;
            }
            _uof.OrderRepository.Create(order);

            await _cartService.CleanCart(userInfo.UserId);

            return _mapper.Map<OrderResponseDTO>(order);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }



    public async Task<OrderResponseDTO> ConfirmOrder(int orderId, decimal amountPaid)
    {
        // if (order.Status != Status.PaidWaitingStoreAcceptance)
        //return null;

        var order = await GetOrderByIdNoTracking(orderId);

        order.Status = Status.Confirmed;
        order.AmountPaid = amountPaid;


        _uof.OrderRepository.Update(_mapper.Map<Order>(order));
        await _uof.CommitAsync();

        return order;
    }

    public async Task<OrderResponseDTO> ReadyForPickup(int orderId)
    {
        var order = await GetOrderByIdNoTracking(orderId);

        if (order.Status != Status.Confirmed)
            return null;

        order.Status = Status.ReadyForPickup;
        _uof.OrderRepository.Update(_mapper.Map<Order>(order));
        await _uof.CommitAsync();

        return _mapper.Map<OrderResponseDTO>(order);
    }

    public async Task<OrderResponseDTO> OutForDelivery(int orderId)
    {
        var order = await GetOrderByIdNoTracking(orderId);
        // if (order.Status != Status.ReadyForPickup)
        // return null;

        order.Status = Status.OutForDelivery;
        _uof.OrderRepository.Update(_mapper.Map<Order>(order));
        await _uof.CommitAsync();

        return _mapper.Map<OrderResponseDTO>(order);
    }

    public async Task<OrderResponseDTO> Delivered(int orderId)
    {
        var order = await GetOrderByIdNoTracking(orderId);
        //if (order.Status != Status.OutForDelivery)
        //return null;

        order.Status = Status.Delivered;
        _uof.OrderRepository.Update(_mapper.Map<Order>(order));
        await _uof.CommitAsync();

        return _mapper.Map<OrderResponseDTO>(order);
    }

    public async Task<OrderResponseDTO> CancelOrder(Order order)
    {
        if (order.Status != Status.PendingPayment && order.Status != Status.Confirmed)
            return null;

        order.Status = Status.Delivered;
        _uof.OrderRepository.Update(order);
        await _uof.CommitAsync();

        return _mapper.Map<OrderResponseDTO>(order);
    }

    public async Task<ReportOrdersResponseDTO> GetReportByMonth(OrdersFilterMonthAndYear parameters, string userId)
    {

        Expression<Func<Order, bool>> filter = null;

        if (parameters.Month > 0 && parameters.Year > 0)
        {
            filter = o =>
                          o.OrderTime.Month == parameters.Month &&
                          o.OrderTime.Year == parameters.Year;
        }

        var queryTotals = _uof.OrderRepository.GetAll().Where(filter);

        var billing = await queryTotals.SumAsync(o => o.TotalAmount);

        var ordersNumber = await queryTotals.CountAsync();

        var pagedOrders = await _uof.OrderRepository.GetPagedAsync(
            filter,
            o => o.OrderTime,
            true,
            parameters.PageNumber,
            parameters.PageSize,
            o => o.OrderItens
        );


        return new ReportOrdersResponseDTO
        {
            Orders = _mapper.Map<PagedList<OrderResponseDTO>>(pagedOrders),
            Billing = billing,
            OrdersTotalNumber = ordersNumber,
            OrdersAverageValue = ordersNumber > 0 ? billing / ordersNumber : 0
        };
    }

    public async Task<DashboardStatsResponseDTO> GetDashboardStats()
    {
        var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6).Date;

        var allOrdersQuery = _uof.OrderRepository.GetAll();

        var totalRevenue = await allOrdersQuery
            .Where(o => o.Status != Status.Canceled)
            .SumAsync(o => o.TotalAmount);

        var totalOrders = await allOrdersQuery.CountAsync();

        var pendingOrders = await allOrdersQuery
            .CountAsync(o => o.Status == Status.PendingPayment);

        var totalProducts = await _uof.ProductRepository.GetAll().CountAsync();


        var historyData = await allOrdersQuery
            .Where(o => o.OrderTime >= sixMonthsAgo && o.Status != Status.Canceled)
            .GroupBy(o => new { o.OrderTime.Year, o.OrderTime.Month })
            .Select(g => new
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Total = g.Sum(x => x.TotalAmount)
            })
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .ToListAsync();

        var monthlyRevenue = new List<decimal>();
        var monthlyLabels = new List<string>();

        for (int i = 5; i >= 0; i--)
        {
            var date = DateTime.UtcNow.AddMonths(-i);
            var stats = historyData.FirstOrDefault(h => h.Month == date.Month && h.Year == date.Year);

            monthlyLabels.Add($"{date.Month}/{date.Year}");
            monthlyRevenue.Add(stats?.Total ?? 0);
        }

        var statusConfirmed = await allOrdersQuery.CountAsync(o => o.Status == Status.Confirmed || o.Status == Status.Delivered);
        var statusPending = await allOrdersQuery.CountAsync(o => o.Status == Status.PendingPayment);
        var statusCanceled = await allOrdersQuery.CountAsync(o => o.Status == Status.Canceled);

        return new DashboardStatsResponseDTO
        {
            TotalRevenue = totalRevenue,
            TotalOrders = totalOrders,
            PendingOrders = pendingOrders,
            TotalProducts = totalProducts,
            MonthlyRevenue = monthlyRevenue,
            MonthlyLabels = monthlyLabels,
            OrderStatusCounts = new List<int> { statusConfirmed, statusPending, statusCanceled }
        };
    }


    public async Task<Order> OrderPermisson(int orderId, string userId)
    {
        var order = await _uof.OrderRepository.GetAsync(o => o.Id == orderId, o => o.OrderItens);

        return order;

    }

    public DateTime GetBrasiliaTime()
    {
        DateTime timeUtc = DateTime.UtcNow;
        try
        {
            TimeZoneInfo kstZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(timeUtc, kstZone);
        }
        catch (TimeZoneNotFoundException)
        {
            TimeZoneInfo kstZone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
            return TimeZoneInfo.ConvertTimeFromUtc(timeUtc, kstZone);
        }
    }




}
