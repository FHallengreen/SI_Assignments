import requests
from dotenv import load_dotenv
import os

# Load environment variables from .env file
load_dotenv()

# Get Ngrok URL and secret key from environment variables
ngrok_url = os.getenv("NGROK_URL")
secret_key = os.getenv("WEBHOOK_SECRET_KEY") 

# Webhook.site URL for receiving payloads
webhook_site_url = "https://webhook.site/6d4522ef-529c-4b61-81f2-23cf4bdf33a8"

# Validate environment variables
if not ngrok_url or not secret_key:
    print("Error: NGROK_URL or WEBHOOK_SECRET_KEY not set in .env file")
    exit(1)

# Payload for registering the webhook - updated to match API requirements
payload = {
    "url": webhook_site_url,
    "eventType": "payment.processed"
}

# Headers for the request
headers = {
    "accept": "*/*",
    "Content-Type": "application/json"
}

# Register the webhook with corrected endpoint
try:
    response = requests.post(f"{ngrok_url}/webhooks/register", json=payload, headers=headers)
    print(f"Status Code: {response.status_code}")
    if response.text:
        print(f"Response: {response.json()}")
    else:
        print("No response body received")
except requests.RequestException as e:
    print(f"Error registering webhook: {e}")
except ValueError as e:
    print(f"Error parsing JSON response: {e}")
    if hasattr(response, 'text'):
        print(f"Response text: {response.text}")