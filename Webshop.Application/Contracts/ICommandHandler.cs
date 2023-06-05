using MediatR;
using Webshop.Domain.Common;

namespace Webshop.Application.Contracts
{
    public interface ICommandHandler<TCommand>
       : IRequestHandler<TCommand, Result> where TCommand : ICommand
    {
        new Task<Result> Handle(TCommand command, CancellationToken cancellationToken = default);

    }
}
