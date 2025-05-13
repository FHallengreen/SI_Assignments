namespace webhook.Models;

public class Webhook
{
    public int Id { get; set; }
    public required string CallbackUrl { get; set; }
    public required string EventType { get; set; }
    public required string SecretKey { get; set; }
}