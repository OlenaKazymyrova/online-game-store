namespace OnlineGameStore.DAL.Entities;

public class License
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public string? Description { get; set; }
    public decimal Cost { get; set; }

    public Game? Game { get; set; }

}