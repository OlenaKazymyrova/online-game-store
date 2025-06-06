using Microsoft.AspNetCore.Mvc;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;

namespace OnlineGameStore.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly IGameService _service;

    public GamesController(IGameService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Retrieves a game by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the game to retrieve.</param>
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var game = await _service.GetByIdAsync(id);
        if (game == null)
        {
            return NotFound();
        }

        return Ok(game);
    }

    /// <summary>
    /// Retrieves a list of all games.
    /// </summary>
    [ProducesResponseType(typeof(IEnumerable<GameDto>), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var games = await _service.GetAsync();
        return Ok(games);
    }

    /// <summary>
    /// Creates a new game.
    /// </summary>
    /// <param name="gameDto">The game data to create.</param>
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] GameDto? gameDto)
    {
        if (gameDto == null)
        {
            return BadRequest("Game data is required.");
        }

        var createdGame = await _service.AddAsync(gameDto);
        if (createdGame == null)
        {
            return BadRequest("Failed to create the game.");
        }

        return Created($"api/Games/{createdGame.Id}", createdGame);
    }

    /// <summary>
    /// Deletes a game by its ID.
    /// </summary>
    /// <param name="id">The ID of the game to delete.</param>
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}