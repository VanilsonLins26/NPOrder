namespace NP_Encomendas_BackEnd.DTOs.Response;

public class DashboardStatsResponseDTO
{
    public decimal TotalRevenue { get; set; }
    public int TotalOrders { get; set; }
    public int PendingOrders { get; set; }
    public int TotalProducts { get; set; }

    public List<decimal> MonthlyRevenue { get; set; } = new();
    public List<string> MonthlyLabels { get; set; } = new();

    public List<int> OrderStatusCounts { get; set; } = new();
}
