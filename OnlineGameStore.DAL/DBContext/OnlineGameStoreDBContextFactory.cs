using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;


namespace OnlineGameStore.DAL.DBContext;

public class OnlineGameStoreDbContextFactory : IDesignTimeDbContextFactory<OnlineGameStoreDbContext>
{

    public OnlineGameStoreDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OnlineGameStoreDbContext>();

        // use real connection string here when applying migrations
        optionsBuilder.UseSqlServer("");

        return new OnlineGameStoreDbContext(optionsBuilder.Options);
    }
}
