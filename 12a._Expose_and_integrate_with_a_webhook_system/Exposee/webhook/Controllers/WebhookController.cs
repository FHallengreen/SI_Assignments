using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webhook.Data;
using webhook.Models;

namespace webhook.Controllers;

[Route("api/webhooks")]
[ApiController]
public class WebhookController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;

    public WebhookController(AppDbContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] WebhookRegistrationRequest request)
    {
        if (string.IsNullOrEmpty(request.CallbackUrl) || string.IsNullOrEmpty(request.EventType) || string.IsNullOrEmpty(request.SecretKey))
        {
            return BadRequest("All fields are required.");
        }

        var supportedEvents = new List<string> { "payment_initiated", "payment_processed", "payment_failed", "payment_refunded" };
        if (!supportedEvents.Contains(request.EventType))
        {
            return BadRequest("Invalid EventType. Supported: payment_initiated, payment_processed, payment_failed, payment_refunded.");
        }

        var existing = await _context.Webhooks
            .FirstOrDefaultAsync(w => w.CallbackUrl == request.CallbackUrl && w.EventType == request.EventType);
        if (existing != null)
        {
            return BadRequest("Webhook already registered.");
        }

        var webhook = new Webhook
        {
            CallbackUrl = request.CallbackUrl,
            EventType = request.EventType,
            SecretKey = request.SecretKey
        };

        _context.Webhooks.Add(webhook);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Webhook registered successfully." });
    }

    [HttpPost("unregister")]
    public async Task<IActionResult> Unregister([FromBody] WebhookUnregistrationRequest request)
    {
        if (string.IsNullOrEmpty(request.CallbackUrl) || string.IsNullOrEmpty(request.EventType))
        {
            return BadRequest("CallbackUrl and EventType are required.");
        }

        var webhook = await _context.Webhooks
            .FirstOrDefaultAsync(w => w.CallbackUrl == request.CallbackUrl && w.EventType == request.EventType);
        if (webhook == null)
        {
            return NotFound("Webhook not found.");
        }

        _context.Webhooks.Remove(webhook);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Webhook unregistered successfully." });
    }

    [HttpPost("ping")]
    public async Task<IActionResult> Ping()
    {
        var webhooks = await _context.Webhooks.ToListAsync();
        var httpClient = _httpClientFactory.CreateClient();
        var tasks = new List<Task>();

        foreach (var webhook in webhooks)
        {
            var payload = new
            {
                EventType = "ping",
                Message = "Test ping event",
                Timestamp = DateTime.UtcNow.ToString("o")
            };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, webhook.CallbackUrl)
            {
                Content = content
            };
            request.Headers.Add("X-Webhook-Secret", webhook.SecretKey);
            tasks.Add(httpClient.SendAsync(request));
        }

        try
        {
            await Task.WhenAll(tasks);
            return Ok(new { Message = $"Ping sent to {webhooks.Count} webhooks." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Some webhooks failed.", Error = ex.Message });
        }
    }
}