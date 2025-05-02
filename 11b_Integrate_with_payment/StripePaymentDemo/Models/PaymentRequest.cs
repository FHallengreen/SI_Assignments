namespace PaymentDemo.Models;

public record PaymentRequest(string Email, decimal Amount, string Currency = "dkk");