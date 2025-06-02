using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Services;

public class GenreService(IGenreRepository repository, IMapper mapper) : IGenreService
{
    private readonly IGenreRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<GenreDto?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);

        return (entity is null) ? null : _mapper.Map<GenreDto>(entity);
    }

    public async Task<IEnumerable<GenreDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<GenreDto>>(entities);
    }

    public async Task<GenreDto?> AddAsync(GenreDto dto)
    {
        if (dto is null)
        {
            return null;
        }

        var entity = _mapper.Map<Genre>(dto);

        return (await _repository.AddAsync(entity) is null) ? null : _mapper.Map<GenreDto>(entity);
    }

    public async Task<bool> UpdateAsync(GenreDto dto)
    {
        if (dto is null)
        {
            return false;
        }

        var entity = _mapper.Map<Genre>(dto);

        return await _repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }
}
