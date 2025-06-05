# Webhook Payment Demo (Exposee)

This is a Web API implementing a webhook system for payment processing events, built with ASP.NET Core for Assignment 12a. It allows Integrators to register webhooks to receive real-time notifications about payment events, similar to GitHub’s webhook system.

## Purpose
The system simulates a payment platform that notifies external applications (Integrators) about payment events, such as when a payment is initiated or processed. Integrators can subscribe to specific event types, receive notifications via HTTP POST requests, and test the system using a ping endpoint.

## Accessing the System
The API is exposed publicly via Ngrok at:
**`<ngrok-url>`** (e.g., `https://abcd-123.ngrok-free.app`)

- **Swagger UI**: Access the interactive API documentation at `<ngrok-url>/swagger`.
- **Note**: The Ngrok URL is temporary and may change. Contact the Exposee for the current URL during testing.

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

### Do following:
1. Go to webhook.site and create a new URL.
2. Open swagger UI at `<ngrok-url>/swagger`.
3. Copy the URL and paste it into the `callbackUrl` field in the request body.
4. Choose an event type from the list of available event types.
5. Generate a secret key using a secure method (e.g., UUID, random string).
6. Paste the secret key into the `secretKey` field in the request body.
7. Send the request to register the webhook.
8. Check the response to confirm successful registration.
9. Try creating a payment event to test the webhook. ("/api/payments"). you can also try the "/ping" endpoint to test the webhook.
10. Check the webhook.site URL to see if you received the notification.