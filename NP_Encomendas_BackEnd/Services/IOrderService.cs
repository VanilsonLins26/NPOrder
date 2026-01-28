using NP_Encomendas_BackEnd.DTOs.Request;
using NP_Encomendas_BackEnd.DTOs.Response;
using NP_Encomendas_BackEnd.Models;
using NP_Encomendas_BackEnd.Pagination;

namespace NP_Encomendas_BackEnd.Services;

public interface IOrderService
{
    Task<PagedList<OrderResponseDTO>> GetAllOrdersPaged(OrderParameters parameters);
    Task<PagedList<OrderResponseDTO>> GetAllOrdersByUser(string userId, OrderParameters parameters);
    Task<OrderResponseDTO> CreateOrder(CreateOrderDTO dto, UserInfoDTO userInfo);
    Task<OrderResponseDTO> GetOrderByIdNoTracking(int orderId);
    Task<OrderResponseDTO> GetOrderById(int orderId);
    Task<DashboardStatsResponseDTO> GetDashboardStats();
    Task<OrderResponseDTO> ConfirmOrder(int orderId, decimal amountPaid);
    Task<Order> OrderPermisson(int orderId, string userId);
    Task<OrderResponseDTO> ReadyForPickup(Order order);
    Task<OrderResponseDTO> OutForDelivery(Order order);
    Task<OrderResponseDTO> Delivered(Order order);
    Task<OrderResponseDTO> CancelOrder(Order order);
    Task<ReportOrdersResponseDTO> GetReportByMonth(OrdersFilterMonthAndYear parameters, string userId);
}
