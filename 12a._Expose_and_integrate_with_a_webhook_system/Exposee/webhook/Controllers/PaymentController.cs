using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhook.Data;
using webhook.Models;

namespace webhook.Controllers;

[Route("api/payments")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;

    public PaymentController(AppDbContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest request)
    {
        if (request.Amount <= 0 || string.IsNullOrEmpty(request.Currency))
        {
            return BadRequest("Invalid amount or currency.");
        }

        var payment = new Payment
        {
            Amount = request.Amount,
            Currency = request.Currency,
            Status = "Initiated",
            CreatedAt = DateTime.UtcNow
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        // Simulate payment processing
        var random = new Random();
        payment.Status = random.Next(0, 3) switch
        {
            0 => "Processed",
            1 => "Failed",
            2 => "Refunded",
            _ => "Processed"
        };
        await _context.SaveChangesAsync();

        // Trigger webhooks for the payment status
        var eventType = payment.Status switch
        {
            "Initiated" => "payment_initiated",
            "Processed" => "payment_processed",
            "Failed" => "payment_failed",
            "Refunded" => "payment_refunded",
            _ => null
        };

        await TriggerWebhooks(eventType ?? throw new InvalidOperationException(), new
        {
            PaymentId = payment.Id,
            Amount = payment.Amount,
            Currency = payment.Currency,
            Status = payment.Status,
            Timestamp = payment.CreatedAt.ToString("o")
        });

        return Ok(new { Message = "Payment processed.", PaymentId = payment.Id, Status = payment.Status });
    }

    private async Task TriggerWebhooks(string eventType, object payload)
    {
        var webhooks = await _context.Webhooks
            .Where(w => w.EventType == eventType)
            .ToListAsync();
        var httpClient = _httpClientFactory.CreateClient();
        var tasks = new List<Task>();

        foreach (var webhook in webhooks)
        {
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, webhook.CallbackUrl)
            {
                Content = content
            };
            request.Headers.Add("X-Webhook-Secret", webhook.SecretKey);
            tasks.Add(httpClient.SendAsync(request));
        }

        await Task.WhenAll(tasks);
    }
}