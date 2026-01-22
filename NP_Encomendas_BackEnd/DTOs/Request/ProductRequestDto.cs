using System.ComponentModel.DataAnnotations;

namespace NP_Encomendas_BackEnd.DTOs.Request;

public class ProductRequestDto
{
    [Required(ErrorMessage = "Informe o nome do produto!!")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "Informe o valor do produto!!")]
    [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "O preço deve ser maior que 0")]
    public decimal? Price { get; set; }
    [Required(ErrorMessage = "Informe a descrição!!")]
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public IFormFile? ImageFile { get; set; }
    [Required(ErrorMessage = "Informe a unidade de medida!!")]
    public string? UnitOfMeasure { get; set; }
    public bool Customizable { get; set; }
}
