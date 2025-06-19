using OnlineGameStore.BLL.DTOs.Genres;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Interfaces;

public interface IGenreService : IService<Genre, GenreCreateDto, GenreReadDto, GenreDto, GenreReadDto> { }