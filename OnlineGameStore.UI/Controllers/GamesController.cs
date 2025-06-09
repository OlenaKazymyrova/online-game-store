using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.SharedLogic.Pagination;

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
    /// Retrieves a list of games using pagination.
    /// </summary>
    /// <param name="pagingParams"> Specifies the pageSize and page pagination parameters.</param>
    [ProducesResponseType(typeof(IEnumerable<GameDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] PagingParams pagingParams)
    {
        var paginatedResponse = await _service.GetAsync(pagingParams: pagingParams);

        return (paginatedResponse is null) ? StatusCode(500) : Ok(paginatedResponse);
    }

    /// <summary>
    /// Creates a new game.
    /// </summary>
    /// <param name="gameDto">The game data to create.</param>
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] GameCreateDto? gameDto)
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

    /// <summary>
    /// Updates all game fields.
    /// </summary>
    /// <param name="id">The unique identifier of the Game to update.</param>
    /// <param name="gameDto">The new Game data.</param>
    [HttpPut]
    public async Task<IActionResult> UpdatePutAsync([FromRoute] Guid id, [FromBody] GameCreateDto gameDto)
    {
        if (gameDto == null)
        {
            return BadRequest("Game data is required.");
        }

        var isUpdated = await _service.UpdateAsync(id, gameDto);

        if (!isUpdated)
        {
            return NotFound();
        }

        return Ok();
    }

    /// <summary>
    /// Updates only specified game fields.
    /// </summary>
    /// <remarks>
    /// If you want to update game name and price, you can send a JSON object like this:
    /// 
    ///     [
    ///         {
    ///             "path": "/name",
    ///             "op": "replace",
    ///             "value": "game"
    ///         },
    ///         {
    ///             "path": "/price",
    ///             "op": "replace",
    ///             "value": 5
    ///         }
    ///     ]
    /// 
    /// </remarks>
    /// <param name="id">ID of a Game entity to update</param>
    /// <param name="patchDoc">JSON object that contains fields to be updated and new values</param>
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdatePatchAsync(Guid id, [FromBody] JsonPatchDocument<GameDto> patchDoc)
    {
        if (patchDoc == null) return BadRequest("Patch document is required.");

        var result = await _service.PatchAsync(id, patchDoc);

        if (!result) return NotFound();
        return Ok();
    }
}