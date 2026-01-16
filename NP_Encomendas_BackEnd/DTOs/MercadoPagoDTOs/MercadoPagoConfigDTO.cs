using NP_Encomendas_BackEnd.Models;
using System.Text.Json.Serialization;

namespace NP_Encomendas_BackEnd.DTOs.MercadoPagoDTOs;

public class MercadoPagoConfigDTO
{
    [JsonPropertyName("action")]
    public string Action { get; set; }
    [JsonPropertyName("api_version")]
    public string ApiVersion { get; set; }
    [JsonPropertyName("data")]
    public Data Data { get; set; }
    [JsonPropertyName("date_created")]
    public string DateCreated { get; set; }
    [JsonPropertyName("id")]
    public long Id { get; set; }
    [JsonPropertyName("live_mode")]
    public bool LiveNode { get; set; }
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("user_id")]
    public long UserId { get; set; }
}
