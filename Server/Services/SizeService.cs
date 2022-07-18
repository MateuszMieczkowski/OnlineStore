using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SneakersBase.Server.Entities;
using SneakersBase.Server.Services.Exceptions;
using SneakersBase.Shared.Models;

namespace SneakersBase.Server.Services
{
    public interface ISizeService
    {
        IEnumerable<SizeDto> GetAll();
        SizeDto Create(CreateSizeDto dto);
        void Remove(int id);
        SizeDto Update(int id, UpdateSizeDto dto);
    }

    public class SizeService : ISizeService
    {
        private readonly SneakersDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<SizeService> _logger;

        public SizeService(SneakersDbContext dbContext, IMapper mapper, ILogger<SizeService> logger)
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

        public SizeDto Create(CreateSizeDto dto)
        {
            if (IsSizeDuplicated(dto.Name))
            {
                throw new DuplicateException();
            }

            var size = _mapper.Map<Size>(dto);

            _dbContext.Sizes.Add(size);
            _dbContext.SaveChanges();

            var sizeDto = _mapper.Map<SizeDto>(size);
            return sizeDto;
        }

        public void Remove(int id)
        {
            var size = GetSizeById(id);

            _dbContext.Sizes.Remove(size);
            _dbContext.SaveChanges();
        }

        public SizeDto Update(int id, UpdateSizeDto dto)
        {
            var size = GetSizeById(id);

            IsSizeDuplicated(size.Name);

            size.Name = dto.Name;

            _dbContext.Sizes.Update(size);
            _dbContext.SaveChanges();

            var updatedDto = _mapper.Map<SizeDto>(size);
            return updatedDto;
        }

        private Size GetSizeById(int id)
        {
            var size = _dbContext.Sizes.FirstOrDefault(s => s.Id == id);
            if (size is null)
            {
                throw new NotFoundException("Size not found");
            }
            return size;
        }

        private bool IsSizeDuplicated(string name)
        {
            bool isDuplicated = _dbContext.Sizes.Any(s => s.Name == name);
            if(isDuplicated)
                throw new DuplicateSizeException();
            return isDuplicated;
        }

    }
}
