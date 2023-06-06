using AutoMapper;
using Microsoft.Extensions.Logging;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Service.CatalogClient;

namespace Webshop.Order.Application.ClientFeatures.Catalog.UpdateProduct
{
    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
    {
        private ILogger<UpdateProductCommandHandler> _logger;
        private ICatalogApiClient _client;

        public UpdateProductCommandHandler(ILogger<UpdateProductCommandHandler> logger, ICatalogApiClient client, IMapper mapper)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<Result> Handle(UpdateProductCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _client.UpdateProduct(command.Product);
                return Result.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
                return Result.Fail<ProductDto>(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
