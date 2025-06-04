namespace OnlineGameStore.DAL.Entities;

public class Genre
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; } = string.Empty;
    public required string Description { get; set; } = string.Empty;
    public Guid? ParentId { get; set; } = Guid.Empty;
    public Genre? ParentGenre { get; set; } = default;

    public override bool Equals(object? obj) =>
        Equals(obj as Genre);

    public override int GetHashCode() =>
        HashCode.Combine(Id, Name, Description, ParentId);

    public static bool operator ==(Genre? left, Genre? right) =>
        Equals(left, right);

    public static bool operator !=(Genre? left, Genre? right) =>
        !Equals(left, right);

    private bool Equals(Genre? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return Id == other.Id
               && Name == other.Name
               && Description == other.Description
               && ParentId == other.ParentId;
    }
}