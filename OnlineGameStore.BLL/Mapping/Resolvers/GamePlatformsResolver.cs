using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Mapping.Resolvers;

public class GamePlatformsResolver : IValueResolver<GameCreateDto, Game, List<Platform>>
{
    private readonly OnlineGameStoreDbContext _context;

    public GamePlatformsResolver(OnlineGameStoreDbContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context), "OnlineGameStoreDbContext cannot be null.");
        }

        _context = context;
    }

    public List<Platform> Resolve(GameCreateDto source, Game dest, List<Platform> destMember, ResolutionContext ctx)
    {
        if (source.PlatformsIds is null || source.PlatformsIds.Count == 0)
            return [];

        return _context.Platforms
            .Where(platform => source.GenresIds!.Contains(platform.Id))
            .ToList();
    }
}

