using MediatR;
using Webshop.Domain.Common;

namespace Webshop.Application.Contracts
{
    public interface ICommand: IRequest<Result>
    {
    }
}
