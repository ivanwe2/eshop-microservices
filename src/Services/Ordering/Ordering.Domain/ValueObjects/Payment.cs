namespace Ordering.Domain.ValueObjects
{
    public record Payment
    {
        private const int CVVLenght = 3;

        public string? CardName { get; } = string.Empty;
        public string CardNumber { get; } = string.Empty;
        public string Expiration { get; } = string.Empty;
        public string CVV { get; } = string.Empty;
        public int PaymentMethod { get; } 

        protected Payment()
        {
        }
        private Payment(string cardName, string cardNumber, string expiration, string cvv, int paymentMethod)
        {
            CardName = cardName;
            CardNumber = cardNumber;
            Expiration = expiration;
            CVV = cvv;
            PaymentMethod = paymentMethod;
        }

        public static Payment Of(string cardName, string cardNumber, string expiration, string cvv, int paymentMethod)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(cardName);
            ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber);
            ArgumentException.ThrowIfNullOrWhiteSpace(expiration);
            ArgumentException.ThrowIfNullOrWhiteSpace(cvv);
            ArgumentOutOfRangeException.ThrowIfNotEqual(cvv.Length, CVVLenght);

            return new Payment(cardName, cardNumber, expiration, cvv, paymentMethod);
        }
    }
}
