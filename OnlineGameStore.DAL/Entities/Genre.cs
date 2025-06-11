using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.DAL.Entities;

public class Genre : TEntity
{
    [Required]
    public override Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public required string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
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
        if (other is null)
            return false;

        return ReferenceEquals(this, other) || Id == other.Id;
    }
}