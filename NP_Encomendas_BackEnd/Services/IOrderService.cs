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
    Task<OrderResponseDTO> ReadyForPickup(int orderId);
    Task<OrderResponseDTO> OutForDelivery(int orderId);
    Task<OrderResponseDTO> Delivered(int orderId);
    Task<OrderResponseDTO> CancelOrder(Order orderId);
    Task<ReportOrdersResponseDTO> GetReportByMonth(OrdersFilterMonthAndYear parameters, string userId);
}
