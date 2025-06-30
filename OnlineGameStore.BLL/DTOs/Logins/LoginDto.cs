using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.DTOs.Logins;

public class LoginDto
{
    [Required] 
    public required string Email { get; set; } // check in regex 
    
    [Required]
    public required string Password { get; set; }
    
}