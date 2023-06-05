using AutoMapper;
using Microsoft.Extensions.Logging;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Service.CatalogClient;
using Webshop.Service.CatalogClient.Models.Responses.Internal;
using Webshop.Service.CustomerClient.Models.Responses.Internal;

namespace Webshop.Order.Application.ClientFeatures.Catalog.GetProduct
{
    public class GetProductQueryHandler : IQueryHandler<GetProductQuery, ProductDto>
    {
        private ILogger<GetProductQueryHandler> _logger;
        private IMapper _mapper;
        private ICatalogApiClient _catalogApiClient;

        public GetProductQueryHandler(ILogger<GetProductQueryHandler> logger, IMapper mapper, ICatalogApiClient catalogApiClient)
        {
            _logger = logger;
            _mapper = mapper;
            _catalogApiClient = catalogApiClient;
        }

        public async Task<Result<ProductDto>> Handle(GetProductQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                CatalogProductResponse response = await _catalogApiClient.GetProduct(query.ProductId);
                return _mapper.Map<ProductDto>(response.Product);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
                return Result.Fail<ProductDto>(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
