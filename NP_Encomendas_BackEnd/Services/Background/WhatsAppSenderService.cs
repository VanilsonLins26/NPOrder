using Microsoft.EntityFrameworkCore;
using NP_Encomendas_BackEnd.Context;

namespace NP_Encomendas_BackEnd.Services.Background;

public class WhatsAppSenderService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<WhatsAppSenderService> _logger;

    public WhatsAppSenderService(IServiceProvider serviceProvider, ILogger<WhatsAppSenderService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Serviço de Fila de WhatsApp Iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var evolutionService = scope.ServiceProvider.GetRequiredService<WhatsAppService>();

                   
                    var messagesToSend = await context.NotificationQueues
                        .Where(q => !q.Sent && q.Attempts < 5)
                        .OrderBy(q => q.CreatedAt)
                        .Take(10) 
                        .ToListAsync(stoppingToken);

                    foreach (var msg in messagesToSend)
                    {
                        try
                        {
                            msg.Attempts++;

                         
                            await evolutionService.SendMessageViaApi(msg.Phone, msg.Message);


                            msg.Sent = true;
                            msg.LastError = null;
                            _logger.LogInformation($"Enviado para {msg.Phone}");
                        }
                        catch (Exception ex) { 
                        
                            msg.LastError = ex.Message;
                            _logger.LogError($"Falha final para {msg.Phone}: {ex.Message}");
                        }
                    }

                    if (messagesToSend.Any())
                    {
                        await context.SaveChangesAsync(stoppingToken);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro crítico no loop do WhatsAppSender");
            }


            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
