

using NP_Encomendas_BackEnd.Pagination;

namespace NP_Encomendas_BackEnd.DTOs.Response;

public class ReportOrdersResponseDTO
{
    public PagedList<OrderResponseDTO> Orders { get; set; }
    public decimal Billing { get; set; }
    public decimal OrdersAverageValue { get; set; }
    public int OrdersTotalNumber { get; set; }
}
