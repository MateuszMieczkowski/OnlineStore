using AutoMapper;
using OnlineStore.Server.Entities;
using OnlineStore.Shared.Models;

namespace OnlineStore.Server;

public class SneakersMappingProfile : Profile
{
    public SneakersMappingProfile()
    {
        // CreateMap<CreateProductSizeDto, ProductSize>()
        //     .ForMember(m => m.Size,
        //         c => c.MapFrom(s =>
        //             !s.SizeId.HasValue && !string.IsNullOrEmpty(s.Size) ? new Size { Name = s.Size } : null));

        CreateMap<CreateProductDto, Product>();

        CreateMap<Size, SizeDto>();
        CreateMap<ProductSize, ProductSizeDto>()
            .ForMember(m => m.Size, c => c.MapFrom(s => s.Size.Name));
        CreateMap<Product, ProductDto>()
            .ForMember(m => m.Quantity, c => c.MapFrom(s => s.AvailableSizes.Sum(e => e.Quantity)));

        // CreateMap<CreateSizeDto, Size>();
        CreateMap<UpdateSizeDto, Size>();

        CreateMap<User, AuthResponse>();
    }
}