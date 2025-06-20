using Microsoft.AspNetCore.Mvc;
using OnlineGameStore.BLL.DTOs.Users;
using OnlineGameStore.BLL.Interfaces;

namespace OnlineGameStore.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="userCreateDto">Username, email and password for the new user</param>
    /// <returns>Email and username if registration is successful</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(UserReadDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterUser([FromBody] UserCreateDto userCreateDto)
    {
        var responseDto = await _userService.AddAsync(userCreateDto);
        if (responseDto == null)
            return BadRequest("User registration failed.");

        return CreatedAtAction(nameof(RegisterUser), responseDto);
    }
}