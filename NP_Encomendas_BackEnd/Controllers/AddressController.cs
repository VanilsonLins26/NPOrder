using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NP_Encomendas_BackEnd.DTOs.Request;
using NP_Encomendas_BackEnd.DTOs.Response;
using NP_Encomendas_BackEnd.Models;
using NP_Encomendas_BackEnd.Services;
using System.Formats.Asn1;
using System.Security.Claims;

namespace NP_Encomendas_BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AddressController : ControllerBase
{
    private readonly IAddressService _service;

    public AddressController(IAddressService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AddressResponseDTO>>> GetAllUserAddress()
    {
        var userId = GetUserId();
        var addresses = await _service.GetAllUserAdress(userId);
        return Ok(addresses);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AddressResponseDTO>> GetAddressById(int id)
    {
        var userId = GetUserId();
        var address = await _service.GetAddressById(id, userId);
        if (address == null)
            return BadRequest("Endereço não existe ou pertence a outro usuário");

        return Ok(address);
    }

    [HttpPost]
    public async Task<ActionResult<AddressResponseDTO>> CreateAddress([FromBody] AddressRequestDTO addressDto)
    {
        var userId = GetUserId();
        var addressCreated = await _service.CreateAddress(addressDto, userId);

        return Ok(addressCreated);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AddressResponseDTO>> UpdateAddress([FromBody] AddressRequestDTO addressDto, int id)
    {
        var userId = GetUserId();
        var addressUpdated = await _service.UpdateAddress(addressDto, id, userId);
        if (addressUpdated == null)
            return BadRequest("Endereço não existe ou pertence a outro usuário");

        return Ok(addressUpdated);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<AddressResponseDTO>> DeletedAddress(int id)
    {
        var userId = GetUserId();
        var addressDeleted = await _service.DeleteAddress(id, userId);

        if (addressDeleted == null)
            return BadRequest("Endereço não existe ou pertence a outro usuário");

        return Ok(addressDeleted);
    }

    [HttpPut("setdefault/{id:int}")]
    public async Task<ActionResult> SetDefault(int id)
    {
        var userId = GetUserId();
        var defaultAddress = await _service.SetDefault(id, userId);

        if (defaultAddress == true)
            return Ok(defaultAddress);

        return BadRequest();
    }


    private string? GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
