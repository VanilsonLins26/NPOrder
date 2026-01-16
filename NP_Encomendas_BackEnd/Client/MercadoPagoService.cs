using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Error;
using MercadoPago.Resource.Payment;
using MercadoPago.Resource.Preference;
using NP_Encomendas_BackEnd.DTOs.MercadoPagoDTOs;
using NP_Encomendas_BackEnd.Models;
using System.Text.Json;

namespace NP_Encomendas_BackEnd.Client;

public class MercadoPagoService
{
    private readonly ILogger<MercadoPagoService> _logger;

    public MercadoPagoService(ILogger<MercadoPagoService> logger)
    {
        _logger = logger;
    }

    public async Task<CreateResponseDTO> CreatePreference(CreatePreferenceRequestDTO inputData)
    {
        try
        {
            PreferenceClient preferenceClient = new PreferenceClient();
            var items = new List<PreferenceItemRequest>();

            if (inputData.PercentToPay == 100)
            {
                foreach (var itemData in inputData.Items)
                {
                    var item = new PreferenceItemRequest
                    {
                        Id = itemData.Id,
                        Title = itemData.Title,
                        Quantity = itemData.Quantity,
                        UnitPrice = itemData.UnitPrice,
                        CurrencyId = "BRL",



                    };
                    items.Add(item);
                }
            }
            else
            {
                var item = new PreferenceItemRequest
                {
                    Id = "sinal_" + inputData.OrderId,
                    Title = $"Sinal (50%) - Pedido #{inputData.OrderId}",
                    Quantity = 1,
                    UnitPrice = inputData.TotalAmount / 2,
                    CurrencyId = "BRL"
                };
                items.Add(item);
            }


            PreferencePayerRequest payer = new PreferencePayerRequest
            {
                Name = inputData.Payer.Name,
                Email = inputData.Payer.Email
            };

            PreferenceBackUrlsRequest backUrlsRequest = new PreferenceBackUrlsRequest
            {
                Success = "https://localhost:4200/payment/success",
                Failure = "https://localhost:4200/payment/failure",
                Pending = "https://localhost:4200/payment/pending"

            };

            PreferenceRequest preferenceRequest = new PreferenceRequest
            {
                ExpirationDateTo =DateTime.UtcNow.AddHours(4).AddMinutes(10),
                Items = items,
                Payer = payer,
                BackUrls = backUrlsRequest,
                ExternalReference = inputData.OrderId.ToString(),
                AutoReturn = "approved"

            };
            _logger.LogInformation("Criando preferência com urls: Success: {preference.BackUrls.Success}, Failure: {preference.BackUrls.Failure}, Pending: {preference.BackUrls.Pending} ", preferenceRequest.BackUrls.Success, preferenceRequest.BackUrls.Failure, preferenceRequest.BackUrls.Pending);
            Preference preference = await preferenceClient.CreateAsync(preferenceRequest);
            
            return new CreateResponseDTO
            {
                PreferenceId = preference.Id,
                RedirectUrl = preference.InitPoint
            };
        }
        catch (MercadoPagoApiException apiException)
        {
            string conteudoErro = apiException.ApiError != null
                ? JsonSerializer.Serialize(apiException.ApiError)
                : "Sem detalhes";

            _logger.LogError("Erro API MP. Status: {StatusCode}. Conteúdo: {Content}",
                apiException.StatusCode, conteudoErro);

            throw;
        }

        catch (Exception e)
        {
            _logger.LogError(e, "Erro inesperado ao criar preferência");

            throw new MercadoPagoException("Erro inesperado ao criar preferência", e);
        }
    }

    public async Task<PaymentEntity> GetPaymentStatus(long id)
    {
        PaymentClient paymentClient = new PaymentClient();
        Payment paymentMercadoPago = await paymentClient.GetAsync(id);

        if (paymentMercadoPago == null)
        {
            throw new MercadoPagoException("Pagamento não encontrado");
        }

        string status = paymentMercadoPago.Status;
        string paymentMethods = paymentMercadoPago.PaymentMethodId;

        Payer payer = null;

        if (paymentMercadoPago.Payer != null && paymentMercadoPago.Payer.Identification != null)
        {
            var payerMP = paymentMercadoPago.Payer;
            payer = new Payer
            {
                Email = payerMP.Email,
                FirstName = payerMP.FirstName,
                LastName = payerMP.LastName,
                Identification = new Identification
                {
                    Type = payerMP.Identification.Type,
                    Number = payerMP.Identification.Number
                }
            };

        }

        return new PaymentEntity
        {
            Id = paymentMercadoPago.Id.ToString(),
            OrderId = paymentMercadoPago.ExternalReference,
            Status = status,
            Amount = paymentMercadoPago.TransactionAmount?.ToString(System.Globalization.CultureInfo.InvariantCulture),
            Payer = payer,
            PaymentMethodId = paymentMethods
        };
    }

}
