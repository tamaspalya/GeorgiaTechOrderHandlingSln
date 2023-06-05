using Microsoft.Extensions.Logging;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Order.Application.Contracts.Persistence;

namespace Webshop.Order.Application.Features.UpdateOrder
{
    public class UpdateOrderCommandHandler: ICommandHandler<UpdateOrderCommand>
    {
        private ILogger<UpdateOrderCommandHandler> _logger;
        private IOrderRepository _orderRepository;

        public UpdateOrderCommandHandler(ILogger<UpdateOrderCommandHandler> logger, IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task<Result> Handle(UpdateOrderCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                await _orderRepository.UpdateAsync(command.Order);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
                return Result.Fail(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
