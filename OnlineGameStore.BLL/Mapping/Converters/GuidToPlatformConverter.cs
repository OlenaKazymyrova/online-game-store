using AutoMapper;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Mapping.Converters;
public class GuidToPlatformConverter : ITypeConverter<Guid, Platform>
{
    private readonly OnlineGameStoreDbContext _context;

    public GuidToPlatformConverter(OnlineGameStoreDbContext context)
    {
        _context = context;
    }

    public Platform Convert(Guid source, Platform destination, ResolutionContext context)
    {
        var dbSet = _context.Set<Platform>();
        var platform = dbSet.Find(source);
        if (platform is not null)
            return platform;

        throw new KeyNotFoundException($"Platform with ID [{source}] was not found.");
    }
}