using Webshop.Service.PaymentClient.Models.Requests;
using Webshop.Service.PaymentClient.Models.Responses.Internal;

namespace Webshop.Service.PaymentClient
{
    public interface IPaymentApiClient
    {
        public Task<ProcessPaymentResponse> ProcessPayment(PaymentRequest paymentRequest);
    }
}
