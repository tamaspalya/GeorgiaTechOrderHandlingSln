using Webshop.Application.Contracts;
using Webshop.Order.Application.Features.Dto;

namespace Webshop.Order.Application.Features.GetOrders
{
    public class GetOrdersQuery: IQuery<List<OrderDto>>
    {
    }
}
