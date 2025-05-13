namespace webhook.Models;

public class Webhook
{
    public int Id { get; set; }
    public string CallbackUrl { get; set; }
    public string EventType { get; set; }
    public string SecretKey { get; set; }
}