using AutoMapper;
using Webshop.Order.Application.Features.CreateOrder;
using Webshop.Order.Application.Features.Dto;
using Webshop.Order.Application.Features.UpdateOrder;

namespace Webshop.Order.Application.Profiles
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.AggregateRoots.Order, OrderDto>().ReverseMap();
            CreateMap<Domain.AggregateRoots.Order, CreateOrderRequest>().ReverseMap();
            CreateMap<Domain.AggregateRoots.Order, UpdateOrderRequest>().ReverseMap();
        }
    }
}
