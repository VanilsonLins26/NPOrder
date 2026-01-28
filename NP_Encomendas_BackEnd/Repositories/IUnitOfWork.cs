using Microsoft.EntityFrameworkCore.Migrations;

namespace NP_Encomendas_BackEnd.Repositories;

public interface IUnitOfWork
{
    IProductRepository ProductRepository { get; }
    ICartRepository CartRepository { get; }
    IOrderRepository OrderRepository { get; }
    IAddressRepository AddressRepository { get; }
    IPaymentRepository PaymentRepository { get; }



    Task CommitAsync();
}
