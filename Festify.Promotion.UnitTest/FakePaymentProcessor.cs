namespace Festify.Promotion.UnitTest;

internal class FakePaymentProcessor : IPaymentProcessor
{
    public record Payment(string CreditCardNumber, decimal Total);
    private readonly List<Payment> payments = new List<Payment>();
    
    public Task ProcessCreditCardPayment(string creditCardNumber, decimal total)
    {
        payments.Add(new Payment(creditCardNumber, total));
        return Task.CompletedTask;
    }

    public IReadOnlyList<Payment> Payments => payments;
}