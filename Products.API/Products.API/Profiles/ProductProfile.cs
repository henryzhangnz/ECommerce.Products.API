using AutoMapper;
using Products.API.Dtos;
using Products.API.Models;

namespace Products.API.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductReadDto, Product>();
            CreateMap<Product, ProductReadDto>();
            CreateMap<ProductCreateDto, Product>();
        }
    }
}
