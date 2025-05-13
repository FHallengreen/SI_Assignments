namespace webhook.Models;

public class Payment
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public required string Currency { get; set; }
    public required string Status { get; set; } // e.g., Initiated, Processed, Failed, Refunded
    public DateTime CreatedAt { get; set; }
}