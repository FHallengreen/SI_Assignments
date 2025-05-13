# Webhook Payment Demo (Exposee)

This is a Web API implementing a webhook system for payment processing events, built with ASP.NET Core for Assignment 12a. It allows Integrators to register webhooks to receive real-time notifications about payment events, similar to GitHub’s webhook system.

## Purpose
The system simulates a payment platform that notifies external applications (Integrators) about payment events, such as when a payment is initiated or processed. Integrators can subscribe to specific event types, receive notifications via HTTP POST requests, and test the system using a ping endpoint.

## Accessing the System
The API is exposed publicly via Ngrok at:
**`<ngrok-url>`** (e.g., `https://abcd-123.ngrok-free.app`)

- **Swagger UI**: Access the interactive API documentation at `<ngrok-url>/swagger`.
- **Note**: The Ngrok URL is temporary and may change. Contact the Exposee for the current URL during testing.

## Prerequisites for Integrator
- A tool to send HTTP requests (e.g., Postman, curl, or Swagger UI).
- A callback URL to receive webhook payloads:
  - Use [webhook.site](https://webhook.site/) to get a free, temporary URL (recommended for testing).
  - Alternatively, set up your own public server to receive HTTP POST requests.
- An internet connection to access the Ngrok URL and webhook.site.

## Theme
The webhook system is themed around payment processing, with the following event types:
- `payment_initiated`: A payment has been created.
- `payment_processed`: A payment was successfully completed.
- `payment_failed`: A payment failed.
- `payment_refunded`: A payment was refunded.

## Endpoints

### Register Webhook
`POST <ngrok-url>/api/webhooks/register`
- **Purpose**: Subscribe to a payment event type to receive notifications at a callback URL.
- **Body**:
  ```json
  {
    "callbackUrl": "https://webhook.site/9821f989-da09-4932-83ee-9b34a793f82d",
    "eventType": "payment_processed",
    "secretKey": "my_secret_key"
  }