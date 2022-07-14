using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SneakerBase.Shared.Dtos;

namespace SneakersBase.Server.Services
{
    public interface ISizeService
    {
        IEnumerable<SizeDto> GetAll();
    }

    public class SizeService : ISizeService
    {
        private readonly SneakersDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public SizeService(SneakersDbContext dbContext, IMapper mapper, ILogger<ProductService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public IEnumerable<SizeDto> GetAll()
        {
            var sizes = _dbContext.Sizes.ToList();
            var sizeDtos = _mapper.Map<List<SizeDto>>(sizes);
            return sizeDtos;
        }

    }
}
