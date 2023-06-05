using Microsoft.Extensions.Logging;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Order.Application.Contracts.Persistence;

namespace Webshop.Order.Application.Features.DeleteOrder
{
    public class DeleteOrderCommandHandler : ICommandHandler<DeleteOrderCommand>
    {
        private ILogger<DeleteOrderCommandHandler> _logger;
        private IOrderRepository _orderRepository;

        public DeleteOrderCommandHandler(ILogger<DeleteOrderCommandHandler> logger, IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task<Result> Handle(DeleteOrderCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                await _orderRepository.DeleteAsync(command.OrderId);
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
