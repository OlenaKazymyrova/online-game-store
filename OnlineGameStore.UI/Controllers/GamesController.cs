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
    /// Returns a Game entity by its ID.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET api/Games/fd5f3b67-0187-48ee-a50e-afdfd1ce5830
    ///
    /// Returns:
    /// 
    ///     {
    ///         "id": fd5f3b67-0187-48ee-a50e-afdfd1ce5830,
    ///         "name": "Sample Game",
    ///         "description": "This is a sample game description.",
    ///         "publisherId": "05de777a-05fb-4792-bab5-0b171b6e5be4",
    ///         "genreId": "7d606f4b-e3c8-4d89-8b24-8dc121945bf6",
    ///         "licenseId": "259fe8d4-840e-4680-b969-aefc7f527118",
    ///         "price": 29.99,
    ///         "releaseDate": "2023-10-01T00:00:00"
    ///     }
    /// </remarks>
    /// <param name="id">ID of a Game entity to get</param>
    /// <returns>Game with specified ID if exists</returns>
    /// <response code="200">Returns the Game entity</response>
    /// <response code="404">If the Game with specified ID does not exist</response>
    [ProducesResponseType(typeof(GameDto), 200)]
    [ProducesResponseType(typeof(void), 404)]
    [Produces("application/json")]
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
    /// Returns list of all Game entities.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET api/Games
    ///
    /// Returns:
    /// 
    ///     [
    ///         {
    ///             "id": "fd5f3b67-0187-48ee-a50e-afdfd1ce5830",
    ///             "name": "Sample Game 1",
    ///             "description": "This is a sample game description.",
    ///             "publisherId": "05de777a-05fb-4792-bab5-0b171b6e5be4",
    ///             "genreId": "7d606f4b-e3c8-4d89-8b24-8dc121945bf6",
    ///             "licenseId": "259fe8d4-840e-4680-b969-aefc7f527118",
    ///             "price": 29.99,
    ///             "releaseDate": "2023-10-01T00:00:00"
    ///         },
    ///         {
    ///             "id": "a7e4d5cc-3c7d-4cf4-b23a-eaf91c03b192",
    ///             "name": "Sample Game 2",
    ///             "description": "Another sample game description.",
    ///             "publisherId": "1c2d3e4f-5678-4f9b-9abc-def123456789",
    ///             "genreId": "abcde123-4567-8901-2345-abcdef678901",
    ///             "licenseId": "76543210-fedc-ba98-7654-3210fedcba98",
    ///             "price": 49.99,
    ///             "releaseDate": "2024-06-15T00:00:00"
    ///         }
    ///     ]
    /// </remarks>
    /// <returns>List of all Game entities</returns>
    /// <response code="200">Returns list of all Game entities</response>
    [ProducesResponseType(typeof(IEnumerable<GameDto>), 200)]
    [Produces("application/json")]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var games = await _service.GetAllAsync();
        return Ok(games);
    }

    /// <summary>
    /// Creates a new Game entity.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST api/Games/
    ///
    ///     {
    ///        "name": "string",
    ///        "description": "string",
    ///        "publisherId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///        "genreId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///        "licenseId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///        "price": 33,
    ///        "releaseDate": "2025-06-01T17:02:14.791Z"
    ///     }
    /// 
    /// Returns:
    /// 
    ///     {
    ///        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa8",
    ///        "name": "string",
    ///        "description": "string",
    ///        "publisherId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///        "genreId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///        "licenseId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///        "price": 33,
    ///        "releaseDate": "2025-06-01T17:02:14.791Z"
    ///     }
    /// </remarks>
    /// <param name="gameDto">
    ///     GameDto object with required fiels
    ///     <param>Name of the game
    ///         <name>name</name>
    ///     </param>
    ///     <param>Game's price
    ///         <name>price</name>
    ///     </param>
    ///     <param>Date of game's release
    ///         <name>releaseDate</name>
    ///     </param>
    /// </param>
    /// <returns>Game with specified ID if exists</returns>
    /// <response code="201">Returns created Game entity</response>
    /// <response code="400">Returns when request does not contain required data</response>
    [ProducesResponseType(typeof(GameDto), 201)]
    [ProducesResponseType(typeof(string), 400)]
    [Produces("application/json")]
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
    /// Deletes a Game entity by its ID.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     DELETE api/Games/3fa85f64-5717-4562-b3fc-2c963f66afa8
    ///
    /// </remarks>
    /// <param name="id">ID of the Game to delete</param>
    /// <response code="204">Returns when Game with specified ID has been successfully deleted</response>
    /// <response code="404">Returns when Game with specified ID is not found</response>
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(void), 404)]
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