using Moq;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.UI.Tests.Interfaces;

namespace OnlineGameStore.UI.Tests.ServiceMockCreators;

public class GenreServiceMockCreator : ServiceMockCreator<Genre, GenreDto, IGenreService>
{
    public GenreServiceMockCreator(List<GenreDto> data) : base(data) { }
}
