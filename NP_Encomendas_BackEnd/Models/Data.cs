using System.Text.Json.Serialization;

namespace NP_Encomendas_BackEnd.Models;

public class Data
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}
