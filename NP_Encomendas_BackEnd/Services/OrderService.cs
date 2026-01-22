using NP_Encomendas_BackEnd.DTOs.Request;
using NP_Encomendas_BackEnd.DTOs.Response;
using NP_Encomendas_BackEnd.Models;
using NP_Encomendas_BackEnd.Pagination;
using NP_Encomendas_BackEnd.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Connections.Features;

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

    public async Task<PagedList<OrderResponseDTO>> GetAllOrdersByUser(string userId, string role, OrderParameters parameters)
    {
        var orders = new PagedList<Order>();
        if (role.Equals("Client"))
        {
             orders = await _uof.OrderRepository.GetPagedAsync(o => o.ClientId == userId, o => o.DeliverTime, true,
                            parameters.PageNumber, parameters.PageSize, o => o.OrderItens, o => o.Address);
        }
        else if (role.Equals("Admin"))
        {
            Expression<Func<Order, bool>>? predicate = null;
            bool descending = true;
            if (parameters.ViewModel == OrderViewModel.Active)
            {
                predicate = o => o.Status != Status.Delivered && o.Status != Status.Canceled && o.Status != Status.PendingPayment;
            }

            else if (parameters.ViewModel == OrderViewModel.History)
            {
                predicate = o => o.Status == Status.Delivered;
                descending = true;
            }

                orders = await _uof.OrderRepository.GetPagedAsync(predicate, o => o.DeliverTime, descending,
                                parameters.PageNumber, parameters.PageSize, o => o.OrderItens, o => o.Address);
        }

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
        var order = await _uof.OrderRepository.GetAsync(o => o.Id == orderId, o => o.OrderItens, o => o.Address);
        if (order == null)
            return null;

        return _mapper.Map<OrderResponseDTO>(order);
    }


    public async Task<OrderResponseDTO> CreateOrder(CreateOrderDTO dto, UserInfoDTO userInfo )
    {
     
        try
        {
           

            var cartDTO = await _cartService.GetCartByUserIdAsync(userInfo.UserId);

            if (!cartDTO.CartItems.Any())
                return null;

           

            var order = new Order
            {
                ClientId = userInfo.UserId,
                DeliverTime = dto.DeliveryTime,
                Status = Status.PendingPayment,
                UserName = userInfo.Name,
                Phone = userInfo.Phone,
                AddressId = dto.DeliveryMethod ==DeliveryMethod.Delivery ? dto.AddressId : null,
                DeliveryMethod = dto.DeliveryMethod
              

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

    public async Task<OrderResponseDTO> ReadyForPickup(Order order)
    {
        if (order.Status != Status.Confirmed)
            return null;

        order.Status = Status.ReadyForPickup;
        _uof.OrderRepository.Update(order);
        await _uof.CommitAsync();

        return _mapper.Map<OrderResponseDTO>(order);
    }

    public async Task<OrderResponseDTO> OutForDelivery(Order order)
    {
       // if (order.Status != Status.ReadyForPickup)
           // return null;

        order.Status = Status.OutForDelivery;
        _uof.OrderRepository.Update(order);
        await _uof.CommitAsync();

        return _mapper.Map<OrderResponseDTO>(order);
    }

    public async Task<OrderResponseDTO> Delivered(Order order)
    {
        //if (order.Status != Status.OutForDelivery)
            //return null;

        order.Status = Status.Delivered;
        _uof.OrderRepository.Update(order);
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


    public async Task<Order> OrderPermisson (int orderId, string userId)
    {
        var order = await _uof.OrderRepository.GetAsync(o => o.Id == orderId, o => o.OrderItens);

            return order;

    }

    


}
