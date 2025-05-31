namespace OnlineGameStore.BLL.DTOs;

public class GenreDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public Guid? ParentId { get; set; }

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
        if (ReferenceEquals(this, other)) return true;

        return Id == other.Id &&
            Name == other.Name &&
            Description == other.Description &&
            ParentId == other.ParentId;
    }
}
