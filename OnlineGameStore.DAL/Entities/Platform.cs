using OnlineGameStore.DAL.Entities;

public class Platform : TEntity
{
    public override Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public ICollection<Game> Games { get; set; } = new List<Game>();
}