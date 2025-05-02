using Microsoft.Extensions.Configuration;
using Stripe;
using Microsoft.OpenApi.Models;
using dotenv.net;

// Load .env file
DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Stripe
var stripeSecretKey = builder.Configuration.GetValue<string>("SecretKey");
if (string.IsNullOrEmpty(stripeSecretKey))
{
    throw new Exception("Stripe SecretKey is not configured in .env");
}
StripeConfiguration.ApiKey = stripeSecretKey;

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Payment Demo API",
        Version = "v1",
        Description = "API for testing Stripe payment integration"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseStaticFiles(); // Serve static files from wwwroot
app.UseHttpsRedirection();
app.UseAuthorization();

// Add Swagger middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment Demo API v1");
    c.RoutePrefix = "swagger"; // Serve Swagger UI at /swagger
});

app.MapControllers();

app.Run();