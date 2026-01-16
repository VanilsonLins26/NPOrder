using System.ComponentModel.DataAnnotations;

namespace NP_Encomendas_BackEnd.DTOs.Response;

public class AddressResponseDTO
{
    public int Id { get; set; }
    [Required]
    public string UserId { get; set; }
    public string Street { get; set; }
    public string Number { get; set; }
    public string? Complement { get; set; }
    public string District { get; set; }
    public string ZipCode { get; set; }
    public bool IsDefault { get; set; }
}
