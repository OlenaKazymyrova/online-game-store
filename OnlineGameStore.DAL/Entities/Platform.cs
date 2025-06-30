namespace OnlineGameStore.DAL.Entities;

public class Platform : Entity
{
    public override Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public ICollection<Game> Games { get; set; } = new List<Game>();

    public override bool Equals(object? obj) =>
        Equals(obj as Platform);

    public override int GetHashCode() =>
        HashCode.Combine(Id, Name);

    public static bool operator ==(Platform? left, Platform? right) =>
        Equals(left, right);

    public static bool operator !=(Platform? left, Platform? right) =>
        !Equals(left, right);

    private bool Equals(Platform? other)
    {
        if (other is null)
            return false;

        return ReferenceEquals(this, other) || Id == other.Id;
    }

}