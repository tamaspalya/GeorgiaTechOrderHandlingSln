using System.Windows.Input;
using Webshop.Domain.Common;

namespace Webshop.Application.Contracts
{
    public interface IDispatcher
    {
        Task<Result<T>> Dispatch<T>(IQuery<T> query);
        Task<Result> Dispatch(ICommand command);
    }
}
