using MediatR;
using Webshop.Domain.Common;

namespace Webshop.Application.Contracts
{
    public interface IQuery<T>: IRequest<Result<T>>
    {
    }
}
