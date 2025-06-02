using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.DBContext;

namespace OnlineGameStore.DAL.Repositories;

public class GenreRepository : Repository<Genre>, IGenreRepository
{ 
    public GenreRepository(OnlineGameStoreDbContext context) : base(context) {}
    
    
    public override async Task<Genre?> AddAsync(Genre entity)
    {
        if (entity.ParentId is not null
            && entity.ParentId != Guid.Empty
            && await DbSet.FindAsync(entity.ParentId) is null)
        {
            return null;
        }

        try
        {
            await DbSet.AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error adding genre: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return null;
        }
    }

    
    
    
    public override async Task<bool> UpdateAsync(Genre entity)
    {
        
        var existingGenre = await DbSet.FindAsync(entity.Id);
        
        if (existingGenre == null)
        {
            return false;
        }
        
        if (entity.ParentId.HasValue)
        {
            var parentExists = await DbSet.AnyAsync(g => g.Id == entity.ParentId.Value);
            if (!parentExists)
            {
                return false;
            }
        }
        
        DbContext.Entry(existingGenre).CurrentValues.SetValues(entity);
    
        return  await DbContext.SaveChangesAsync() > 0;;
    }
    
    
}