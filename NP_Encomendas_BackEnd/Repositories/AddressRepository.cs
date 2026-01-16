using Microsoft.EntityFrameworkCore;
using NP_Encomendas_BackEnd.Context;
using NP_Encomendas_BackEnd.Models;

namespace NP_Encomendas_BackEnd.Repositories;

public class AddressRepository : Repository<Address>, IAddressRepository
{
    public AddressRepository(AppDbContext context) : base(context)
    {

    }

    public async Task<int> CountUserAddress(string userId)
    {
        return await _context.Addresses.CountAsync(a => a.UserId == userId);
       
    }

    public  IQueryable<Address> GetAllUserAddress(string userId)
    {
        return _context.Addresses.Where(a => a.UserId == userId).AsQueryable();   
    }
}
