using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.DTOs.Users;

public class UserCreateDto
{
    [Required]
    public required string Username { get; set; }
    [Required]
    public required string Email { get; set; }
    [Required]
    public required string Password { get; set; }
}