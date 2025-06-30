using Microsoft.AspNetCore.Mvc;
using OnlineGameStore.BLL.DTOs.Logins;
using OnlineGameStore.BLL.DTOs.Users;
using OnlineGameStore.BLL.Interfaces;

namespace OnlineGameStore.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _service;

    public AuthController(IUserService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
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
        try
        {
            var responseDto = await _service.AddAsync(userCreateDto);
            return CreatedAtAction(nameof(RegisterUser), responseDto);
        }
        catch (ArgumentException ex) when (ex.ParamName == nameof(UserCreateDto.Email))
        {
            return BadRequest(new {  Message = "An account with this email already exists." });
        }
        catch (ArgumentException ex) when (ex.ParamName == nameof(UserCreateDto.Password))
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (ArgumentException ex) when (ex.ParamName == nameof(UserCreateDto.Username))
        {
            return BadRequest(new {  Message ="Username already exists." });
        }
    }
    
    // ADD here documentation
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var tokenResult = await _service.LoginAsync(loginDto);
        if (tokenResult is null)
        {
            return BadRequest("Invalid credentials");
        }
        HttpContext.Response.Cookies.Append("refreshToken", tokenResult.RefreshToken);
        
        return Ok(new { token = tokenResult.AccessToken });
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrWhiteSpace(refreshToken))
            return BadRequest("No refresh token found.");

        var result = await _service.RefreshTokenAsync(refreshToken);
        if (result is null)
            return BadRequest("Invalid or expired refresh token");

        HttpContext.Response.Cookies.Append("refreshToken", result.RefreshToken);

        return Ok(new { token = result.AccessToken });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("refreshToken");
        return Ok("Logged out successfully");
    }
}