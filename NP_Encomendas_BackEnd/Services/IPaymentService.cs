using NP_Encomendas_BackEnd.DTOs.Request;
using NP_Encomendas_BackEnd.DTOs.Response;
using NP_Encomendas_BackEnd.Models;
using NP_Encomendas_BackEnd.Pagination;

namespace NP_Encomendas_BackEnd.Services;

public interface IPaymentService
{
    Task<PagedList<PaymentResponseDTO>> GetPagedAllPayment(PaymentParameters parameters);
    Task<IEnumerable<PaymentResponseDTO>> GetPayment(int id);
    Task<PaymentResponseDTO> CreatePayment(PaymentRequestDTO payment);
    Task<PaymentResponseDTO> UpdatePayment(int id, PaymentRequestDTO paymentDto);
    Task<PaymentResponseDTO> GetPaymentNoTracking(int id);
}
