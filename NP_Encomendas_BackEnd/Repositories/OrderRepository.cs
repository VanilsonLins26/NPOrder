using Microsoft.EntityFrameworkCore;
using NP_Encomendas_BackEnd.Context;
using NP_Encomendas_BackEnd.Models;
using System.Security.Cryptography.X509Certificates;

namespace NP_Encomendas_BackEnd.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context)
    {
       
    }

    public async Task<Order> GetOrderByIdNoTracking(int orderId)
    {
        return await _context.Orders.Where(o => o.Id == orderId).AsNoTracking().Include(o => o.OrderItens).Include(o => o.Address).FirstOrDefaultAsync();
    }


}
