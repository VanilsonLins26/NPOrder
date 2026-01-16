using MercadoPago.Error;
using NP_Encomendas_BackEnd.Client;
using NP_Encomendas_BackEnd.DTOs.MercadoPagoDTOs;
using System.Runtime.ConstrainedExecution;

namespace NP_Encomendas_BackEnd.Services;

public class CreatePaymentPreferenceService
{
    private readonly MercadoPagoService _client;
    private readonly ILogger<CreatePaymentPreferenceService> _logger;

    public CreatePaymentPreferenceService(MercadoPagoService client, ILogger<CreatePaymentPreferenceService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<CreateResponseDTO> createPreference(CreatePreferenceRequestDTO inputData)
    {
        _logger.LogInformation("Creating payment preference with request : {inputData}", inputData);

       
        try
        {
            return await _client.CreatePreference(inputData);
        }
        catch(MercadoPagoException e)
        {
            _logger.LogError("Erro ao criar preferência de pagamento: {Message}", e.Message);
            throw new MercadoPagoException(e);

        }
        catch(MercadoPagoApiException e)
        {
            _logger.LogError("Erro ao criar pagamento: {}", e.Message);
            throw new MercadoPagoException(e);
        }
    }
}
