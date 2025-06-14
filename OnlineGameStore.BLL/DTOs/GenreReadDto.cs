using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.DTOs;

public class GenreReadDto
{
    [Required]
    public required Guid Id { get; set; }

    [Required]
    public required string Name { get; set; }

    public string Description { get; set; } = string.Empty;

    public Guid? ParentId { get; set; } = default;
    [Required]
    public ICollection<Guid> GamesIds { get; set; } = new List<Guid>();

    public override bool Equals(object? obj) =>
        Equals(obj as GenreReadDto);

    public override int GetHashCode() =>
        HashCode.Combine(Id, Name, Description, ParentId);

    public static bool operator ==(GenreReadDto? left, GenreReadDto? right) =>
        Equals(left, right);

    public static bool operator !=(GenreReadDto? left, GenreReadDto? right) =>
        !Equals(left, right);

    private bool Equals(GenreReadDto? other)
    {
        if (other is null)
            return false;

        return ReferenceEquals(this, other) || Id == other.Id;
    }
}
