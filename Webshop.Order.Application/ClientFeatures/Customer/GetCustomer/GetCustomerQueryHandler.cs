using AutoMapper;
using Microsoft.Extensions.Logging;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Service.CustomerClient;
using Webshop.Service.CustomerClient.Models.Responses.Internal;

namespace Webshop.Order.Application.ClientFeatures.Customer.GetCustomer
{
    public class GetCustomerQueryHandler : IQueryHandler<GetCustomerQuery, CustomerDto>
    {
        private ILogger<GetCustomerQueryHandler> _logger;
        private IMapper _mapper;
        private ICustomerApiClient _customerApiClient;

        public GetCustomerQueryHandler(ILogger<GetCustomerQueryHandler> logger, IMapper mapper, ICustomerApiClient customerApiClient)
        {
            _logger = logger;
            _mapper = mapper;
            _customerApiClient = customerApiClient;
        }

        public async Task<Result<CustomerDto>> Handle(GetCustomerQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                GetCustomerResponse response = await _customerApiClient.GetCustomer(query.CustomerId);
                return _mapper.Map<CustomerDto>(response.Customer);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
                return Result.Fail<CustomerDto>(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
