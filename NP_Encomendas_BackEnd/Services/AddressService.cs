using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NP_Encomendas_BackEnd.DTOs.Request;
using NP_Encomendas_BackEnd.DTOs.Response;
using NP_Encomendas_BackEnd.Models;
using NP_Encomendas_BackEnd.Repositories;
using System.Runtime.InteropServices.Marshalling;
using System.Xml.Linq;

namespace NP_Encomendas_BackEnd.Services;

public class AddressService : IAddressService
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public AddressService(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AddressResponseDTO>> GetAllUserAdress(string userId)
    {
        var addresses = await _uof.AddressRepository.GetAllUserAddress(userId).ToListAsync() ;
        return _mapper.Map<IEnumerable<AddressResponseDTO>>(addresses);
    }

    public async Task<AddressResponseDTO> GetAddressById(int id, string loggedUserId)
    {

        var address = await _uof.AddressRepository.GetAsync(a => a.Id == id);
        if (address is null)
            return null;

        if (address.UserId != loggedUserId)
            return null;

        return _mapper.Map<AddressResponseDTO>(address);
    }

    public async Task<AddressResponseDTO> CreateAddress(AddressRequestDTO addressDto, string loggedUserId)
    {
        var maxAddress = 5;
        var addressCount = await _uof.AddressRepository.CountUserAddress(loggedUserId);
        if (addressCount >= maxAddress)
            return null;

        var address = _mapper.Map<Address>(addressDto);
        address.UserId = loggedUserId;

        if (addressCount == 0)
        {
            address.IsDefault = true;
        }

        var addressCreated = _uof.AddressRepository.Create(address);
        await _uof.CommitAsync();

        return _mapper.Map<AddressResponseDTO>(addressCreated);
    }

    public async Task<AddressResponseDTO> UpdateAddress(AddressRequestDTO addressDto,int id, string loggedUserId)
    {
        var address = await _uof.AddressRepository.GetAsync(a => a.Id == id);
        if (address.UserId != loggedUserId)
            return null;

        _mapper.Map(addressDto, address);
        var addressUpdated = _uof.AddressRepository.Update(address);
        await _uof.CommitAsync();

        return _mapper.Map<AddressResponseDTO>(addressUpdated);

        
    }
    public async Task<bool> SetDefault(int id, string loggedUserId)
    {

        var defaulAddress = await _uof.AddressRepository.GetAsync(a => a.IsDefault == true && a.UserId == loggedUserId);
        if (defaulAddress != null)
        {
            defaulAddress.IsDefault = false;
            _uof.AddressRepository.Update(defaulAddress);
        }

        var newDefault = await _uof.AddressRepository.GetAsync(a => a.Id == id);
        if (newDefault == null || newDefault.UserId != loggedUserId)
            return false;

        newDefault.IsDefault = true;
        _uof.AddressRepository.Update(newDefault);

        await _uof.CommitAsync();

        return true;


      
    }


    public async Task<AddressResponseDTO> DeleteAddress(int id, string loggedUserId)
    {
        var address = await _uof.AddressRepository.GetAsync(a => a.Id == id);
        if (address == null || address.UserId != loggedUserId)
            return null;

        var addressDeleted = _uof.AddressRepository.Delete(address);

        await _uof.CommitAsync();

        return _mapper.Map<AddressResponseDTO>(addressDeleted);
    }
}
