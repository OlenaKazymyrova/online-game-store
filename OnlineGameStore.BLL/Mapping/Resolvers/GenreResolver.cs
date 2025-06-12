using AutoMapper;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.Migrations;

namespace OnlineGameStore.BLL.Mapping.Resolvers;

public class GenreResolver : 
    IValueResolver<GenreCreateDto, Genre, ICollection<Game>>,
    IValueResolver<Genre, GenreReadDto, ICollection<Guid>>
{
    private readonly OnlineGameStoreDbContext _context;

    public GenreResolver(OnlineGameStoreDbContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context), "OnlineGameStoreDbContext cannot be null.");
        }
        _context = context;
    }

    public ICollection<Game> Resolve(GenreCreateDto source, Genre dest, ICollection<Game> destMember, ResolutionContext ctx)
    {
        if (source.GamesIds is null || source.GamesIds.Count == 0)
            return [];

        var gameIds = source.GamesIds.ToArray();

        return _context.Games
            .Where(game => gameIds!.Contains(game.Id))
            .ToList();
    }

    public ICollection<Guid> Resolve(Genre source, GenreReadDto dest, ICollection<Guid> destMember, ResolutionContext ctx)
    {
        if (source.Games is null || source.Games.Count == 0)
            return [];

        return source.Games
            .Select(game => game.Id)
            .ToList();
    }
}