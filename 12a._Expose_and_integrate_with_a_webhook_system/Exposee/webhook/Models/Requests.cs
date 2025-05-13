namespace webhook.Models;

public class WebhookRegistrationRequest
{
    public required string CallbackUrl { get; set; }
    public required string EventType { get; set; }
    public required string SecretKey { get; set; }
}

public class WebhookUnregistrationRequest
{
    public required string CallbackUrl { get; set; }
    public required string EventType { get; set; }
}

public class PaymentRequest
{
    public decimal Amount { get; set; }
    public required string Currency { get; set; }
}