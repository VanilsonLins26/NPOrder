

namespace NP_Encomendas_BackEnd.Models;

public class Payer
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Identification Identification { get; set; }
}
