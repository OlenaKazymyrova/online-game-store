using AutoMapper;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Mapping.Converters;
public class GuidToGameConverter : ITypeConverter<Guid, Game>
{
    private readonly OnlineGameStoreDbContext _context;

    public GuidToGameConverter(OnlineGameStoreDbContext context)
    {
        _context = context;
    }

    public Game Convert(Guid source, Game destination, ResolutionContext context)
    {
        var dbSet = _context.Set<Game>();
        var game = dbSet.Find(source);
        if (game is not null)
            return game;

        throw new KeyNotFoundException($"Game with ID [{source}] was not found.");
    }
}