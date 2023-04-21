namespace Festify.Promotion.UnitTest;

internal interface IPaymentProcessor
{
    Task ProcessCreditCardPayment(string creditCardNumber, decimal total);
}