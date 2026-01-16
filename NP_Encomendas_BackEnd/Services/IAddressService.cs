using NP_Encomendas_BackEnd.DTOs.Request;
using NP_Encomendas_BackEnd.DTOs.Response;

namespace NP_Encomendas_BackEnd.Services;

public interface IAddressService
{
    Task<IEnumerable<AddressResponseDTO>> GetAllUserAdress(string userId);
    Task<AddressResponseDTO> GetAddressById(int id, string loggedUserId);
    Task<AddressResponseDTO> CreateAddress(AddressRequestDTO addressDto, string loggedUserId);
    Task<AddressResponseDTO> UpdateAddress(AddressRequestDTO addressDto, int id, string loggedUserId);
    Task<AddressResponseDTO> DeleteAddress(int id, string loggedUserId);
    Task<bool> SetDefault(int id, string loggedUserId);


}
