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

namespace OnlineGameStore.UI.Tests.ServiceMockCreators;

public class GenreServiceMockCreator : ServiceMockCreator<Genre, GenreCreateDto, GenreReadDto, GenreDto, GenreReadDto, IGenreService>
{
    public GenreServiceMockCreator(List<Genre> data) : base(data) { }

    protected virtual void SetupGet(Mock<IGenreService> mock)
    {
        mock.Setup(x => x.GetAsync(
                It.IsAny<Expression<Func<Genre, bool>>?>(),
                It.IsAny<Func<IQueryable<Genre>, IOrderedQueryable<Genre>>?>(),
                It.IsAny<Func<IQueryable<Genre>, IIncludableQueryable<Genre, object>>?>(),
                It.IsAny<PagingParams>(),
                It.IsAny<HashSet<string>?>()))
            .ReturnsAsync((
                Expression<Func<Genre, bool>>? filter,
                Func<IQueryable<Genre>, IOrderedQueryable<Genre>>? orderBy,
                Func<IQueryable<Genre>, IIncludableQueryable<Genre, object>>? include,
                PagingParams? pagingParams,
                HashSet<string>? explicitIncludes) =>
            {
                pagingParams ??= new PagingParams();

                var entities = _data.Select(d => _mapper.Map<Genre>(d)).AsQueryable();

                if (filter != null)
                    entities = entities.Where(filter);

                if (orderBy != null)
                    entities = orderBy(entities);

                var totalCount = entities.Count();

                var pagedEntities = entities
                    .Skip((pagingParams.Page - 1) * pagingParams.PageSize)
                    .Take(pagingParams.PageSize)
                    .ToList();

                var mappedDtos = _mapper.Map<IEnumerable<GenreReadDto>>(
                    pagedEntities,
                    opts =>
                    {
                        opts.Items["IncludeGenres"] = explicitIncludes?.Contains("genres") ?? false;
                        opts.Items["IncludePlatforms"] = explicitIncludes?.Contains("platforms") ?? false;
                    }
                );

                var mappedDtosWithInclude = mappedDtos.Select(genre =>
                {
                    if (include is null)
                        genre.GamesIds = null;

                    return genre;
                });

                return new PaginatedResponse<GenreReadDto>
                {
                    Items = mappedDtosWithInclude,
                    Pagination = new PaginationMetadata
                    {
                        Page = pagingParams.Page,
                        PageSize = pagingParams.PageSize,
                        TotalItems = totalCount,
                        TotalPages = (int)Math.Ceiling((double)totalCount / pagingParams.PageSize)
                    }
                };
            });
    }

    protected override void SetupDelete(Mock<IGenreService> mock)
    {
        mock.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) =>
            {
                var index = _data.FindIndex(x => x.Id == id);
                if (index == -1)
                {
                    return false;
                }

                _data.RemoveAt(index);

                var parentRefToRemove = id;

                _data.RemoveAll(g => g.ParentId == parentRefToRemove);

                return true;
            });
    }
}