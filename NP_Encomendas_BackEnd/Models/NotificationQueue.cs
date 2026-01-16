using System.ComponentModel.DataAnnotations;

namespace NP_Encomendas_BackEnd.Models;

public class NotificationQueue
{
    [Key]
    public int Id { get; set; }

    public string Phone { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool Sent { get; set; } = false;
    public int Attempts { get; set; } = 0;
    public string? LastError { get; set; }
}
