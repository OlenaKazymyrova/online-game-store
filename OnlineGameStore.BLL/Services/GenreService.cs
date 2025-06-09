using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Services;

public class GenreService : Service<Genre, GenreCreateDto, GenreReadDto, GenreDto>, IGenreService
{
    public GenreService(IGenreRepository repository, IMapper mapper)
        : base(repository, mapper) { }

    //public override async Task<GenreReadDto?> AddAsync(GenreCreateDto dto)
    //{
    //    if (dto == null)
    //        return null;

    //    var entity = _mapper.Map<Genre>(dto);

    //    var addedEntity = await _repository.AddAsync(entity);

    //    return addedEntity == null ? null : _mapper.Map<GenreReadDto>(addedEntity);
    //}

    //public override async Task<bool> UpdateAsync(Guid id, GenreCreateDto dto)
    //{
    //    if (dto is null)
    //        return false;

    //    var entity = _mapper.Map<Genre>(dto);
    //    entity.Id = id;

    //    return await _repository.UpdateAsync

    //}

}
