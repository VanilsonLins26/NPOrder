using AutoMapper;
using NP_Encomendas_BackEnd.DTOs.Request;
using NP_Encomendas_BackEnd.DTOs.Response;
using NP_Encomendas_BackEnd.Helpers;
using NP_Encomendas_BackEnd.Models;
using NP_Encomendas_BackEnd.Pagination;
using NP_Encomendas_BackEnd.Repositories;
using System;
using System.Linq.Expressions;

namespace NP_Encomendas_BackEnd.Services;

public class PaymentService : IPaymentService
{

    private readonly IMapper _mapper;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _uof;

    public PaymentService(IMapper mapper, IPaymentRepository paymentRepository, IUnitOfWork uof)
    {
        _mapper = mapper;
        _paymentRepository = paymentRepository;
        _uof = uof;
    }

    public async Task<PagedList<PaymentResponseDTO>> GetPagedAllPayment(PaymentParameters parameters)
    {
        Expression<Func<PaymentEntity, bool>> filter = null;
        if(parameters.Status != null) 
            filter = p => p.Status == parameters.Status;

        if (parameters.FilterText != null)
            filter = filter.And(p => p.order.UserName.Contains(parameters.FilterText)
                                  || p.Id.ToString() == parameters.FilterText
                                  || p.OrderId.ToString() == parameters.FilterText);

        if (parameters.InitialDate != null)
            filter = filter.And(p => p.DateCreated.Date >= parameters.InitialDate.Value.Date);

        if (parameters.EndDate != null)
            filter = filter.And(p => p.DateCreated.Date <= parameters.EndDate.Value.Date);

        var payments = await _paymentRepository.GetPagedAsync(filter, p => p.DateCreated, true, parameters.PageNumber, parameters.PageSize, p => p.order);
        
        var paymentsDto = _mapper.Map<IEnumerable<PaymentResponseDTO>>(payments);

        var pagedResult = new PagedList<PaymentResponseDTO>(
        paymentsDto.ToList(),
        payments.TotalCount,
        payments.CurrentPage,
        payments.PageSize
    );


        return pagedResult;
    }

    public async Task<IEnumerable<PaymentResponseDTO>> GetPayment(int id)
    {
        var payment = await _paymentRepository.GetAsync(p => p.Id == id, p => p.order);
        return _mapper.Map<IEnumerable<PaymentResponseDTO>>(payment);
    }

    public async Task<PaymentResponseDTO> GetPaymentNoTracking(int id)
    {
        var payment = await _paymentRepository.GetNoTracking(id);
        return _mapper.Map<PaymentResponseDTO>(payment);
    }


    public async Task<PaymentResponseDTO> CreatePayment(PaymentRequestDTO paymentDto)
    {
        var payment = _mapper.Map<PaymentEntity>(paymentDto);
        var paymentCreated = _paymentRepository.Create(payment);
        if (paymentCreated is null)
            return null;

        await _uof.CommitAsync();

        return _mapper.Map<PaymentResponseDTO>(paymentCreated);
    }

    public async Task<PaymentResponseDTO> UpdatePayment(int id, PaymentRequestDTO paymentDto)
    {
        var payment = await _paymentRepository.GetAsync(p => p.Id == id);
        if (payment is null)
            return null;

        _mapper.Map(paymentDto, payment);
        var paymentUpdated = _paymentRepository.Update(payment);
        if (paymentUpdated is null)
            return null;

        await _uof.CommitAsync();

        return _mapper.Map<PaymentResponseDTO>(paymentUpdated);
    }

   
}
