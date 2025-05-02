using Microsoft.AspNetCore.Mvc;

namespace PaymentDemo.Controller;

[Route("api/[controller]")]
[ApiController]
public class StripeConfigController(IConfiguration configuration) : ControllerBase
{
    [HttpGet("publishable-key")]
    public IActionResult GetPublishableKey()
    {
        var key = configuration.GetValue<string>("PublishableKey");
        return Ok(new { PublishableKey = key });
    }
}