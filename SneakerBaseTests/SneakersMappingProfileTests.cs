
using AutoMapper;
using SneakersBase.Server;

namespace SneakerBaseTests
{
    public class SneakersMappingProfileTests
    {
        private readonly IMapper _mapper;

        public SneakersMappingProfileTests()
        {
            //_mapper = new SneakersMappingProfile();
        }

        [Fact]
        public void MapCreateProductSizeDto_ForNullableSize_ReturnsMappedProductSize()
        {
            var productSizeDto = new CreateProductSizeDto()
            {
                Quantity = 100,
                Size = null,
                SizeId = null
            };

         //   var result = _mapper.Map<ProductSize>(productSizeDto);

          //  Assert.True(result.Size == null);

        }
    }
}