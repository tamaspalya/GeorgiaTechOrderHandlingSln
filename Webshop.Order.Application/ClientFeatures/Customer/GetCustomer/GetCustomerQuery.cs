using EnsureThat;
using Webshop.Application.Contracts;

namespace Webshop.Order.Application.ClientFeatures.Customer.GetCustomer
{
    public class GetCustomerQuery: IQuery<CustomerDto>
    {
        public GetCustomerQuery(int customerId)
        {
            Ensure.That(customerId, nameof(customerId)).IsNotDefault<int>();
            Ensure.That(customerId, nameof(customerId)).IsGt<int>(0);
            CustomerId = customerId;
        }

        public int CustomerId { get; private set; }
    }
}
