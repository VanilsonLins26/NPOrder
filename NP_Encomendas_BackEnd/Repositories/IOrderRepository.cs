using NP_Encomendas_BackEnd.Models;

namespace NP_Encomendas_BackEnd.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order> GetOrderByIdNoTracking(int orderId);
    
}
