using AutoMapper;
using ECommerceApp.Product.Application.DTOs;
using ProductEntity = ECommerceApp.Product.Domain.Entities.Product;

namespace ECommerceApp.Product.Application.Mappings
{
    public sealed class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<ProductEntity, ProductDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
