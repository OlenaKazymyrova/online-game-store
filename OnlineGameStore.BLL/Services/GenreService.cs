using AutoMapper;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.BLL.DTOs.Genres;

namespace OnlineGameStore.BLL.Services;

public class GenreService : Service<Genre, GenreCreateDto, GenreReadDto, GenreDto>, IGenreService
{
    public GenreService(IGenreRepository repository, IMapper mapper)
        : base(repository, mapper) { }
}
