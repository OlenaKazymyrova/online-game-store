using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.DAL.DBContext;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Mapping.Resolvers
{
    public class GameGenresResolver : IValueResolver<GameCreateDto, Game, List<Genre>>
    {
        private readonly OnlineGameStoreDbContext _context;

        public GameGenresResolver(OnlineGameStoreDbContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context), "OnlineGameStoreDbContext cannot be null.");
            }

            _context = context;
        }

        public List<Genre> Resolve(GameCreateDto source, Game dest, List<Genre> destMember, ResolutionContext ctx)
        {
            if (source.GenresIds is null || source.GenresIds.Count == 0)
                return [];

            return _context.Genres
                .Where(genre => source.GenresIds!.Contains(genre.Id))
                .ToList();
        }
    }
}
