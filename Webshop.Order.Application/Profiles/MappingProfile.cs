using AutoMapper;
using Webshop.Order.Application.ClientFeatures.Catalog.GetProduct;
using Webshop.Order.Application.ClientFeatures.Customer;
using Webshop.Order.Application.Features.Dto;
using Webshop.Order.Application.Features.Requests;

namespace Webshop.Order.Application.Profiles
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.AggregateRoots.Order, OrderDto>().ReverseMap();
            CreateMap<Domain.AggregateRoots.Order, CreateOrderRequest>().ReverseMap();
            CreateMap<Domain.AggregateRoots.Order, UpdateOrderRequest>().ReverseMap();

            CreateMap<Domain.AggregateRoots.OrderLineItem, OrderLineItemDto>().ReverseMap();
            CreateMap<Domain.AggregateRoots.OrderLineItem, OrderLineItemRequest>().ReverseMap();

            CreateMap<Service.CustomerClient.Models.CustomerDto, CustomerDto>().ReverseMap();
            CreateMap<Service.CatalogClient.Models.ProductDto, ProductDto>().ReverseMap();
        }
    }
}
