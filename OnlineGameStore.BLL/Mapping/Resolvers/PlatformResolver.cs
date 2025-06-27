using AutoMapper;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.BLL.DTOs.Platforms;

namespace OnlineGameStore.BLL.Mapping.Resolvers;

public class PlatformResolver :
    IValueResolver<PlatformCreateDto, Platform, ICollection<Game>>
{
    private readonly OnlineGameStoreDbContext? _context;

    public PlatformResolver()
    {
    }

    public PlatformResolver(OnlineGameStoreDbContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context), "OnlineGameStoreDbContext cannot be null.");
        }

        _context = context;
    }

    public ICollection<Game> Resolve(PlatformCreateDto source, Platform dest, ICollection<Game> destMember,
        ResolutionContext ctx)
    {
        if (source.GamesIds is null || source.GamesIds.Count == 0 || _context is null)
            return [];

        var gameIds = source.GamesIds.ToArray();

        var games = _context.Games
            .Where(game => gameIds!.Contains(game.Id))
            .ToList();

        if (games.Count == gameIds.Length)
            return games;

        var missingIds = gameIds.Except(games.Select(g => g.Id));
        throw new KeyNotFoundException($"Games with IDs [{string.Join(", ", missingIds)}] were not found.");
    }
}