using Microsoft.AspNetCore.Mvc;
using PaymentDemo.Models;
using Stripe;

namespace PaymentDemo.Controllers;

[Route("api/payment")]
[ApiController]
public class PaymentController : ControllerBase
{
    [HttpPost("create-payment")]
    public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest request)
    {
        try
        {
            var customerService = new CustomerService();
            var customer = await customerService.CreateAsync(new CustomerCreateOptions
            {
                Email = request.Email
            });

            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = await paymentIntentService.CreateAsync(new PaymentIntentCreateOptions
            {
                Amount = (long)(request.Amount * 100),
                Currency = request.Currency,
                Customer = customer.Id,
                PaymentMethodTypes = new List<string> { "card" },
                Description = "Test payment"
            });

            return Ok(new { clientSecret = paymentIntent.ClientSecret });
        }
        catch (StripeException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}