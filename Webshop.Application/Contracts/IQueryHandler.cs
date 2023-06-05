using MediatR;
using Webshop.Domain.Common;

namespace Webshop.Application.Contracts
{
    public interface IQueryHandler<TQuery, TResult>
        : IRequestHandler<TQuery, Result<TResult>> where TQuery : IQuery<TResult>
    {
        new Task<Result<TResult>> Handle(TQuery query, CancellationToken cancellationToken = default);
    }
}
