using Microsoft.EntityFrameworkCore.Migrations;
using NP_Encomendas_BackEnd.Context;

namespace NP_Encomendas_BackEnd.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private IProductRepository _productRepo;
    private ICartRepository _cartRepo;
    private IOrderRepository _orderRepository;
    private IAddressRepository _addressRepository;
    private IPaymentRepository _paymentRepository;
    public AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IProductRepository ProductRepository
    {
        get
        {
            return _productRepo = _productRepo ?? new ProductRepository(_context);
  
        }
    }

    public ICartRepository CartRepository
    {
        get
        {
            return _cartRepo = _cartRepo ?? new CartRepository(_context);

        }
    }

    public IOrderRepository OrderRepository
    {
        get
        {
            return _orderRepository = _orderRepository ?? new OrderRepository(_context);

        }
    }
    public IAddressRepository AddressRepository
    {
        get
        {
            return _addressRepository = _addressRepository ?? new AddressRepository(_context);

        }
    }
    public IPaymentRepository PaymentRepository
    {
        get
        {
            return _paymentRepository = _paymentRepository ?? new PaymentRepository(_context);

        }
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}