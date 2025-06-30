using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.DBContext;

namespace OnlineGameStore.DAL.Repositories;

public class GenreRepository(OnlineGameStoreDbContext context) : Repository<Genre>(context), IGenreRepository
{
    public override async Task<Genre?> AddAsync(Genre entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var isParentGenreValid = await IsParentGenreValidAsync(entity.Id, entity.ParentId);

        if (!isParentGenreValid)
        {
            throw new KeyNotFoundException("Parent genre not found.");
        }

        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public override async Task<Genre?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(genre => genre.ParentGenre)
            .Include(genre => genre.Games)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task UpdateGameRefsAsync(Guid id, List<Game> games)
    {
        ArgumentNullException.ThrowIfNull(games);

        var entityToUpdate = await _dbSet.Include(genre => genre.Games).FirstOrDefaultAsync(genre => genre.Id == id);

        if (entityToUpdate is null)
        {
            throw new KeyNotFoundException($"Could not find the Genre with ID {id}");
        }

        entityToUpdate.Games = games;

        await _dbContext.SaveChangesAsync();
    }

    public override async Task<bool> UpdateAsync(Genre entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var existingGenre = await _dbSet.FindAsync(entity.Id);

        if (existingGenre is null)
            throw new KeyNotFoundException("Genre not found.");

        var isParentGenreValid = await IsParentGenreValidAsync(entity.Id, entity.ParentId);

        if (!isParentGenreValid)
        {
            return false;
        }

        _dbContext.Entry(existingGenre).CurrentValues.SetValues(entity);

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public override async Task<bool> DeleteAsync(Guid id)
    {
        var existingGenre = await _dbSet.FindAsync(id);

        if (existingGenre is null)
            throw new KeyNotFoundException("Genre not found.");

        if (existingGenre.ParentId is null)
        {
            var childGenres = _dbSet.Where(g => g.ParentId == existingGenre.Id).ToList();
            _dbSet.RemoveRange(childGenres);
        }

        _dbSet.Remove(existingGenre);

        await _dbContext.SaveChangesAsync();
        return true;
    }

    private async Task<bool> IsParentGenreValidAsync(Guid genreId, Guid? parentId)
    {
        if (parentId is null)
            return true;

        if (parentId == Guid.Empty || genreId == parentId)
            return false;

        return await _dbContext.Genres.FindAsync(parentId) is not null;
    }
}