using System.ComponentModel.DataAnnotations;

namespace NP_Encomendas_BackEnd.DTOs.Request;

public class AddressRequestDTO
{
    public string Street { get; set; }
    public string Number { get; set; }
    public string? Complement { get; set; }
    public string District { get; set; }
    public string ZipCode { get; set; }
}
