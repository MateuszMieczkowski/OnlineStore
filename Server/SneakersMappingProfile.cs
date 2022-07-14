using AutoMapper;
using SneakerBase.Entities;
using SneakerBase.Shared.Dtos;
using SneakersBase.Shared.Dtos;

namespace SneakersBase.Server
{
    public class SneakersMappingProfile : Profile
    {
        public SneakersMappingProfile()
        {
            CreateMap<CreateProductSizeDto, ProductSize>();
            CreateMap<CreateProductDto, Product>();

            CreateMap<Size, SizeDto>();
            CreateMap<ProductSize, ProductSizeDto>()
                .ForMember(m => m.Size, c => c.MapFrom(s => s.Size.Name));
            CreateMap<Product, ProductDto>()
                .ForMember(m => m.Quantity, c => c.MapFrom(s => s.AvailableSizes.Sum(e => e.Quantity)));

        }
    }
}
