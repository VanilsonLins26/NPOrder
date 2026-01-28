using NP_Encomendas_BackEnd.Models;

namespace NP_Encomendas_BackEnd.Repositories;

public interface IPaymentRepository : IRepository<PaymentEntity>
{
    Task<PaymentEntity> GetNoTracking(int id);
}
