async function initializeStripe() {
    const response = await fetch('/api/stripeconfig/publishable-key');
    const { publishableKey } = await response.json();

    const stripe = Stripe(publishableKey);
    const elements = stripe.elements();
    const card = elements.create('card');
    card.mount('#card-element');

    document.querySelector('form').addEventListener('submit', async (e) => {
        e.preventDefault();
        const response = await fetch('/api/payments/create-payment', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email: 'test@example.com', amount: 10.00 })
        });
        const { clientSecret } = await response.json();
        const result = await stripe.confirmCardPayment(clientSecret, {
            payment_method: { card }
        });
        if (result.error) {
            console.error(result.error.message);
        } else {
            console.log('Payment succeeded!');
        }
    });
}

initializeStripe().catch(console.error);