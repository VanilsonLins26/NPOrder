using NP_Encomendas_BackEnd.Models;

namespace NP_Encomendas_BackEnd.Repositories;

public interface IAddressRepository : IRepository<Address>
{
    Task<int> CountUserAddress(string userId);
    IQueryable<Address> GetAllUserAddress(string userId);
}
