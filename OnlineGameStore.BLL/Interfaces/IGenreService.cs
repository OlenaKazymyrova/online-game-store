using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Interfaces;

public interface IGenreService : IService<Genre, GenreDto, GenreReadDto, GenreDto> { }