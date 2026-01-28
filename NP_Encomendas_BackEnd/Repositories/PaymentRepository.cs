using Microsoft.EntityFrameworkCore;
using NP_Encomendas_BackEnd.Context;
using NP_Encomendas_BackEnd.Models;
using System.Security.Cryptography.X509Certificates;

namespace NP_Encomendas_BackEnd.Repositories;

public class PaymentRepository : Repository<PaymentEntity>, IPaymentRepository
{
    public PaymentRepository(AppDbContext context) : base(context)
    {
        
    }

    public async Task<PaymentEntity> GetNoTracking(int id)
    {
        var payment = await _context.Payments.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        return payment;
    }
}
