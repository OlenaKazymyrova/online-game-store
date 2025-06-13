using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Mapping.Resolvers
{
    public class GameResolver :
        IValueResolver<GameCreateDto, Game, ICollection<Genre>>,
        IValueResolver<GameCreateDto, Game, ICollection<Platform>>
    {
        private readonly OnlineGameStoreDbContext? _context;

        public GameResolver()
        { }

        public GameResolver(OnlineGameStoreDbContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context), "OnlineGameStoreDbContext cannot be null.");
            }

            _context = context;
        }

        public ICollection<Genre> Resolve(GameCreateDto source, Game dest, ICollection<Genre> destMember, ResolutionContext ctx)
        {
            if (source.GenresIds is null || source.GenresIds.Count == 0 || _context is null)
                return [];

            var genreIds = source.GenresIds.ToArray();

            var genres = _context.Genres
                .Where(genre => genreIds.Contains(genre.Id))
                .ToList();

            if (genres.Count == genreIds.Length)
                return genres;

            var missingIds = genreIds.Except(genres.Select(g => g.Id));
            throw new KeyNotFoundException($"Genres with IDs: [{string.Join(", ", missingIds)}] were not found.");
        }

        public ICollection<Platform> Resolve(GameCreateDto source, Game dest, ICollection<Platform> destMember, ResolutionContext ctx)
        {
            if (source.PlatformsIds is null || source.PlatformsIds.Count == 0 || _context is null)
                return [];

            var platformIds = source.PlatformsIds.ToArray();

            var platforms = _context.Platforms
                .Where(platform => platformIds!.Contains(platform.Id))
                .ToList();

            if (platforms.Count == platformIds.Length)
                return platforms;

            var missingIds = platformIds.Except(platforms.Select(p => p.Id));
            throw new KeyNotFoundException($"Platforms with IDs: [{string.Join(", ", missingIds)}] were not found.");
        }
    }
}
