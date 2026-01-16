using Microsoft.EntityFrameworkCore;
using NP_Encomendas_BackEnd.Context;
using NP_Encomendas_BackEnd.Models;

namespace NP_Encomendas_BackEnd.Services.Background;

public class OrderCancellationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OrderCancellationService> _logger;

    public OrderCancellationService(IServiceProvider serviceProvider, ILogger<OrderCancellationService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Serviço de Cancelamento Automático Iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();


                    var limiteTempo = DateTime.UtcNow.AddHours(-4);

                    var pedidosVencidos = await context.Orders
                        .Where(o => o.Status == Status.PendingPayment && o.OrderTime <= limiteTempo)
                        .ToListAsync(stoppingToken);

                    if (pedidosVencidos.Any())
                    {
                        foreach (var pedido in pedidosVencidos)
                        {
                            pedido.Status = Status.Canceled;
                            _logger.LogInformation($"Pedido #{pedido.Id} cancelado automaticamente por expiração de tempo.");
                        }

                        await context.SaveChangesAsync(stoppingToken);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao executar o cancelamento automático.");
            }


            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }
}