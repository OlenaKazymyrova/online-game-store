using System.ComponentModel.DataAnnotations;
using OnlineGameStore.SharedLogic.Constants;

namespace OnlineGameStore.BLL.DTOs.Users;

public class UserCreateDto
{
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = UserConstants.PasswordMinLength,
        ErrorMessage = "Password must be at least 8 characters long.")]
    public required string Password { get; set; }
}