using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;


namespace OnlineGameStore.DAL.DBContext;

public class OnlineGameStoreDbContextFactory : IDesignTimeDbContextFactory<OnlineGameStoreDbContext>
{
    public OnlineGameStoreDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OnlineGameStoreDbContext>();

        // use real connection string here when applying migrations
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=OnlineGameStore;User Id=sa;Password=rhIRr5i8R7n39Ckn1QWj;TrustServerCertificate=True;");

        return new OnlineGameStoreDbContext(optionsBuilder.Options);
    }
}