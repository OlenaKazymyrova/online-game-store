using OnlineGameStore.BLL.DTOs.Genres;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Interfaces;

public interface IGenreService : IService<Genre, GenreCreateDto, GenreReadDto, GenreDto, GenreDetailedDto>
{
    public Task UpdateGameRefsAsync(Guid id, List<Guid> gameIds);
}