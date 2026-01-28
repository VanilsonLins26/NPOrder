using NP_Encomendas_BackEnd.Context;
using NP_Encomendas_BackEnd.DTOs.Response;
using NP_Encomendas_BackEnd.Models;
using System.Text;
using System.Text.Json;

namespace NP_Encomendas_BackEnd.Services;

public class WhatsAppService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IOrderService _orderService;
    private readonly AppDbContext _context;


    public WhatsAppService(HttpClient httpClient, IConfiguration configuration, IOrderService orderService, AppDbContext context)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _orderService = orderService;
        _context = context;
    }

    public async Task SendOrderNotificationAsync(int orderId)
    {


        var order = await _orderService.GetOrderByIdNoTracking(orderId);

        var statusName = order.Status switch
        {
            Status.PendingPayment => "Pagamento Pendente",
            Status.Confirmed => "Pedido Confirmado"
        };

        if (order == null) return;

        var sb = new StringBuilder();

        sb.AppendLine("📦 *NOVA ENCOMENDA RECEBIDA!*");
        sb.AppendLine("──────────────────");
        sb.AppendLine($"🔢 *Pedido:* #{order.Id}");
        sb.AppendLine($"👤 *Cliente:* {order.UserName}");
        sb.AppendLine($"📞 *Tel:* {order.Phone}");
        sb.AppendLine($"📅 *Data:* {order.DeliverTime:dd/MM/yyyy} às {order.DeliverTime:HH:mm}");


        sb.AppendLine("");

        if (order.DeliveryMethod == DeliveryMethod.Delivery)
        {
            sb.AppendLine("🚚 *TIPO: ENTREGA (Delivery)*");

            if (order.Address != null)
            {

                sb.AppendLine($"📍 {order.Address.Street}, {order.Address.Number} - {order.Address.District}");


                if (!string.IsNullOrWhiteSpace(order.Address.Complement))
                {
                    sb.AppendLine($"   _Comp: {order.Address.Complement}_");
                }


            }
            else
            {
                sb.AppendLine("⚠️ _Endereço não informado_");
            }
        }
        else
        {

            sb.AppendLine("🏪 *TIPO: RETIRADA NA LOJA*");
            sb.AppendLine("_O cliente irá buscar o pedido._");
        }
        sb.AppendLine("");


        sb.AppendLine("🛒 *ITENS:*");
        foreach (var item in order.OrderItens)
        {
            var productName = item.ProductName ?? "Produto sem nome";
            var itemLine = $"▪️ *{item.Quantity}x* {productName}";

            if (!string.IsNullOrWhiteSpace(item.Comment))
            {
                itemLine += $" _(Obs: {item.Comment})_";
            }

            sb.AppendLine(itemLine);
            sb.AppendLine($"   └ Unit: {item.UnityPrice:C} | Sub: {item.TotalPrice:C}");
        }
        sb.AppendLine("");

        sb.AppendLine("──────────────────");
        sb.AppendLine("💰 *RESUMO FINANCEIRO*");
        sb.AppendLine($"Total: {order.TotalAmount:C}");

        if (order.IsFullyPaid)
        {
            sb.AppendLine("✅ *PAGAMENTO: TOTALMENTE PAGO*");
        }
        else
        {
            sb.AppendLine($"💳 Pago: {order.AmountPaid:C}");
            sb.AppendLine($"⚠️ *RESTANTE A PAGAR: {order.RemainingAmount:C}*");
        }

        sb.AppendLine("──────────────────");
        sb.AppendLine($"_Status Atual: {statusName}_");

        var messageText = sb.ToString();
        var groupJid = _configuration["EvolutionApi:StaffGroupId"];
        var queueItem = new NotificationQueue
        {
            Phone = groupJid,
            Message = messageText,
            CreatedAt = DateTime.UtcNow,
            Sent = false,
            Attempts = 0
        };

        _context.NotificationQueues.Add(queueItem);
        await _context.SaveChangesAsync();
    }

    public async Task SendCustomerConfirmationAsync(int orderId)
    {

        var order = await _orderService.GetOrderByIdNoTracking(orderId);

        if (order == null) return;

        var customerPhone = new string(order.Phone.Where(char.IsDigit).ToArray());

        customerPhone = "55" + customerPhone;

        var sb = new StringBuilder();

        sb.AppendLine($"👋 Olá, *{order.UserName}*! Tudo bem?");
        sb.AppendLine("Recebemos seu pedido com sucesso! 🎉");
        sb.AppendLine("──────────────────");
        sb.AppendLine($"🔢 *Pedido Nº:* #{order.Id}");
        sb.AppendLine($"📅 *Previsão:* {order.DeliverTime:dd/MM/yyyy} às {order.DeliverTime:HH:mm}");

        sb.AppendLine("");

        if (order.DeliveryMethod == DeliveryMethod.Delivery)
        {
            sb.AppendLine("🚚 *Método: ENTREGA*");
            sb.AppendLine("Enviaremos para:");

            if (order.Address != null)
            {
                sb.AppendLine($"📍 {order.Address.Street}, {order.Address.Number} - {order.Address.District}");
                if (!string.IsNullOrWhiteSpace(order.Address.Complement))
                {
                    sb.AppendLine($"   _({order.Address.Complement})_");
                }
            }
        }
        else
        {
            sb.AppendLine("🏪 *Método: RETIRADA*");
            sb.AppendLine("O pedido estará aguardando você na nossa loja.");
        }
        sb.AppendLine("");

  
        sb.AppendLine("──────────────────");
        sb.AppendLine($"💰 *Total do Pedido: {order.TotalAmount:C}*");

        if (order.IsFullyPaid)
        {
            sb.AppendLine("✅ _Pagamento confirmado. Obrigado!_");
        }
        else
        {
            sb.AppendLine($"💳 Valor já pago: {order.AmountPaid:C}");
            sb.AppendLine($"⚠️ *VALOR A PAGAR NA ENTREGA/RETIRADA:*");
            sb.AppendLine($"👉 *{order.RemainingAmount:C}*");
        }

        sb.AppendLine("");
        sb.AppendLine("Qualquer dúvida, é só responder essa mensagem!");

        var messageText = sb.ToString();
        var queueItem = new NotificationQueue
        {
            Phone = customerPhone,
            Message = messageText,
            CreatedAt = DateTime.UtcNow,
            Sent = false,
            Attempts = 0
        };

        _context.NotificationQueues.Add(queueItem);
        await _context.SaveChangesAsync();
    }

    public async Task SendMessageViaApi(string phone, string message)
    {
        var instance = _configuration["EvolutionApi:InstanceName"];

        var cleanPhone = new string(phone.Where(char.IsDigit).ToArray());

        var payload = new { number = cleanPhone, text = message };
        


        var response = await _httpClient.PostAsJsonAsync($"message/sendText/{instance}", payload);

        response.EnsureSuccessStatusCode();
    }
}
