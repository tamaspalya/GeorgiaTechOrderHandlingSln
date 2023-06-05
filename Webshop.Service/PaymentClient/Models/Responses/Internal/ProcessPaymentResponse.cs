namespace Webshop.Service.PaymentClient.Models.Responses.Internal
{
    public class ProcessPaymentResponse
    {
        public bool IsSuccess { get; set; }
        public bool IsFailure { get; set; }
        public string Error { get; set; }
        public string TransactionId { get; set; }
    }
}
