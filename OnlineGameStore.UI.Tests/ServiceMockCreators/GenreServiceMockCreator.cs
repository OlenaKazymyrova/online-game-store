using Microsoft.EntityFrameworkCore.Query;
using Moq;
using OnlineGameStore.BLL.DTOs.Genres;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Services;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.SharedLogic.Pagination;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata;
using OnlineGameStore.BLL.Exceptions;

namespace OnlineGameStore.UI.Tests.ServiceMockCreators;

public class GenreServiceMockCreator : ServiceMockCreator<Genre, GenreCreateDto, GenreReadDto, GenreDto, GenreDetailedDto, IGenreService>
{
    public GenreServiceMockCreator(List<Genre> data) : base(data) { }

    protected override void SetupDelete(Mock<IGenreService> mock)
    {
        mock.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) =>
            {
                var index = _data.FindIndex(x => x.Id == id);
                if (index == -1)
                {
                    throw new NotFoundException("Genre not found.");
                }

                _data.RemoveAt(index);

                var parentRefToRemove = id;

                _data.RemoveAll(g => g.ParentId == parentRefToRemove);

                return true;
            });
    }
}