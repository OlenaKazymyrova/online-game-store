using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Mapping.Resolvers
{
    public class GameCreateDtoResolver : 
        IValueResolver<GameCreateDto, Game, ICollection<Genre>>,
        IValueResolver<GameCreateDto, Game, ICollection<Platform>>
    {
        private readonly OnlineGameStoreDbContext _context;

        public GameCreateDtoResolver(OnlineGameStoreDbContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context), "OnlineGameStoreDbContext cannot be null.");
            }

            _context = context;
        }

        public ICollection<Genre> Resolve(GameCreateDto source, Game dest, ICollection<Genre> destMember, ResolutionContext ctx)
        {
            if (source.GenresIds is null || source.GenresIds.Count == 0)
                return [];

            var genreIds = source.GenresIds.ToArray();

            return _context.Genres
                .Where(genre => genreIds!.Contains(genre.Id))
                .ToList();
        }

        public ICollection<Platform> Resolve(GameCreateDto source, Game dest, ICollection<Platform> destMember, ResolutionContext ctx)
        {
            if (source.PlatformsIds is null || source.PlatformsIds.Count == 0)
                return [];

            var platformIds = source.PlatformsIds.ToArray();

            return _context.Platforms
                .Where(platform => platformIds!.Contains(platform.Id))
                .ToList();
        }
    }
}
