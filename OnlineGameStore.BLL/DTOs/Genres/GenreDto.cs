namespace OnlineGameStore.BLL.DTOs.Genres;

public class GenreDto
{
    public Guid? Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public Guid? ParentId { get; set; }

    public ICollection<Guid> GamesIds = new List<Guid>();

    public override bool Equals(object? obj) =>
        Equals(obj as GenreDto);

    public override int GetHashCode() =>
        HashCode.Combine(Id, Name, Description, ParentId);

    public static bool operator ==(GenreDto? left, GenreDto? right) =>
        Equals(left, right);

    public static bool operator !=(GenreDto? left, GenreDto? right) =>
        !Equals(left, right);

    private bool Equals(GenreDto? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other) || Id == other.Id) return true;

        return Name == other.Name &&
               Description == other.Description &&
               ParentId == other.ParentId;
    }
}