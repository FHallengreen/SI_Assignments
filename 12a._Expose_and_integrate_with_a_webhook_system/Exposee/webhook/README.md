# Webhook System Demo

A simple webhook system built with ASP.NET Core Minimal APIs.

## Getting Started
1. Clone the repository.
2. Run the application: `dotnet run`.
3. Access the Swagger UI at `http://localhost:5293` (or the port shown in the console).
4. Use Swagger to test endpoints or send requests via tools like Postman.

## Testing with Swagger
- Open `http://localhost:5293` in a browser to access the Swagger UI.
- Use the UI to:
  - Register a webhook (`POST /webhooks/register`).
  - Unregister a webhook (`POST /webhooks/unregister`).
  - Trigger a test ping (`POST /webhooks/ping`).
- Example request for registering a webhook:
  ```json
  {
    "callbackUrl": "https://example.com/webhook",
    "eventType": "payment_received",
    "secretKey": "my_secret_key"
  }