async function initializeStripe() {
    try {
        const response = await fetch('/api/stripeconfig/publishable-key');
        if (!response.ok) {
            throw new Error(`Failed to fetch publishable key: ${response.status}`);
        }
        const { publishableKey } = await response.json();

        const stripe = Stripe(publishableKey);
        const elements = stripe.elements();
        const card = elements.create('card', { hidePostalCode: true });
        card.mount('#card-element');

        document.querySelector('#payment-form').addEventListener('submit', async (e) => {
            e.preventDefault();
            try {
                const response = await fetch('/api/payment/create-payment', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ email: 'test@example.com', amount: 10.00 })
                });
                if (!response.ok) {
                    throw new Error(`Failed to create payment: ${response.status}`);
                }
                const { clientSecret } = await response.json();
                const result = await stripe.confirmCardPayment(clientSecret, {
                    payment_method: { card }
                });
                if (result.error) {
                    console.error('Payment failed:', result.error.message);
                    alert(`Payment failed: ${result.error.message}`);
                } else {
                    console.log('Payment succeeded!');
                    alert('Payment succeeded!');
                }
            } catch (error) {
                console.error('Error:', error.message);
                alert(`Error: ${error.message}`);
            }
        });
    } catch (error) {
        console.error('Initialization error:', error.message);
        alert(`Initialization error: ${error.message}`);
    }
}

initializeStripe();
