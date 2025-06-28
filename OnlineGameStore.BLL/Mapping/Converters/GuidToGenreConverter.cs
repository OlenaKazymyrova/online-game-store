using AutoMapper;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Mapping.Converters;

public class GuidToGenreConverter : ITypeConverter<Guid, Genre>
{
    private readonly OnlineGameStoreDbContext _context;

    public GuidToGenreConverter(OnlineGameStoreDbContext context)
    {
        _context = context;
    }

    public Genre Convert(Guid source, Genre destination, ResolutionContext context)
    {
        var dbSet = _context.Set<Genre>();
        var genre = dbSet.Find(source);
        if (genre is not null)
            return genre;

        throw new KeyNotFoundException($"Genre with ID [{source}] was not found.");
    }
}