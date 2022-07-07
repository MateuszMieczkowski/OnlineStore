using AutoMapper;
using SneakerBase.Shared.Dtos;
using WebApplication1.Entities;

namespace SneakersBase.Server
{
    public class SneakersMappingProfile : Profile
    {
        public SneakersMappingProfile()
        {
            CreateMap<Size, SizeDto>();
            CreateMap<ProductSize, ProductSizeDto>()
                .ForMember(m => m.Size, c => c.MapFrom(s => s.Size.Name));
            CreateMap<Product, ProductDto>()
                .ForMember(m => m.Quantity, c => c.MapFrom(s => s.AvailableSizes.Count));
        }
    }
}
