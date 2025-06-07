using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Services;

public class GenreService : Service<Genre, GenreDto, GenreReadDto, GenreDto>, IGenreService
{
    public GenreService(IGenreRepository repository, IMapper mapper)
        : base(repository, mapper) { }

}
