using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Entities;


namespace OnlineGameStore.BLL.Services
{
    public class GenreService(IGenreRepository repository, IMapper mapper) : IGenreService
    {
        private readonly IGenreRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<GenreDto?> GetByIdAsync(Guid id)
        {
            var result = await _repository.GetByIdAsync(id);

            if (result is null)
            {
                return null;
            }

            var resultDto = _mapper.Map<GenreDto>(result);

            return resultDto;
        }

        public async Task<IEnumerable<GenreDto>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync();

            if (result is null)
            {
                return new List<GenreDto>();
            }

            return _mapper.Map<IEnumerable<GenreDto>>(result);
        }

        public async Task<GenreDto?> AddAsync(GenreDto dto)
        {
            var entity = _mapper.Map<Genre>(dto);

            if (entity is null)
            {
                return null;
            }

            var result = await _repository.AddAsync(entity);

            return _mapper.Map<GenreDto>(entity);
        }
        
        public async Task<bool> UpdateAsync(GenreDto dto)
        {
            var entity = _mapper.Map<Genre>(dto);

            if (entity is null)
            {
                return false;
            }

            return await _repository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id); 
        }
    }
}
