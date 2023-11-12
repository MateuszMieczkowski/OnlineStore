using AutoMapper;
using OnlineStore.Server.Entities;
using OnlineStore.Shared.Models;

namespace OnlineStore.Server;

public class SneakersMappingProfile : Profile
{
    public SneakersMappingProfile()
    {
        CreateMap<CreateProductDto, Product>();
        CreateMap<Product, ProductDto>();
        CreateMap<User, AuthResponse>();
    }
}