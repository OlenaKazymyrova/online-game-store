using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using OnlineGameStore.BLL.DTOs.Games;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.SharedLogic.Pagination;
using OnlineGameStore.UI.Aggregation;
using OnlineGameStore.UI.QueryBuilders;

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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var game = await _service.GetByIdAsync(id);
        return Ok(game);
    }

    /// <summary>
    /// Retrieves a list of games using pagination.
    /// </summary>
    /// <param name="pagingParams"> Specifies the pageSize and page pagination parameters.</param>
    /// <param name="aggregationParams"> Specifies the possible filtering and ordering</param>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<GameDetailedDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(
        [FromQuery] PagingParams pagingParams,
        [FromQuery] GameAggregationParams aggregationParams)
    {
        var queryBuilder = new GameQueryBuilder();

        var filter = queryBuilder.BuildFilter(aggregationParams);
        var orderBy = queryBuilder.BuildOrderBy(aggregationParams);
        var include = queryBuilder.BuildInclude(aggregationParams);

        var explicitIncludes = queryBuilder.GetExplicitIncludeSet(aggregationParams);

        var paginatedResponse = await _service.GetAsync(
            include: include,
            filter: filter,
            orderBy: orderBy,
            pagingParams: pagingParams,
            explicitIncludes: explicitIncludes
        );

        return Ok(paginatedResponse);
    }

    /// <summary>
    /// Creates a new game.
    /// </summary>
    /// <param name="gameDto">The game data to create.</param>
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] GameCreateDto gameDto)
    {
        var createdGame = await _service.AddAsync(gameDto);

        return Created($"api/Games/{createdGame.Id}", createdGame);
    }

    /// <summary>
    /// Deletes a game by its ID.
    /// </summary>
    /// <param name="id">The ID of the game to delete.</param>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    /// Updates the list of referenced genres for a specified game.
    /// </summary>
    /// <returns></returns>
    /// <param name="id"> The id go the Game to update.</param>
    /// <param name="genreIds"> The updated list of Genres ids.</param>
    [HttpPut("{id:guid}/genres")]
    public async Task<IActionResult> UpdateGenresAsync([FromRoute] Guid id, [FromBody] List<Guid> genreIds)
    {
        _service
    }

    /// <summary>
    /// Updates the list of referenced genres for a specified game.
    /// </summary>
    /// <returns></returns>
    /// <param name="id"> The id go the Game to update.</param>
    /// <param name="platformIds"> The updated list of Genres ids.</param>
    [HttpPut("{id:guid}/platforms")]
    public async Task<IActionResult> UpdatePlatformsAsync([FromRoute] Guid id, [FromBody] List<Guid> platformIds)
    {
        _service
    }


    /// <summary>
    /// Updates all game fields.
    /// </summary>
    /// <param name="id">The unique identifier of the Game to update.</param>
    /// <param name="gameDto">The new Game data.</param>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePutAsync([FromRoute] Guid id, [FromBody] GameCreateDto gameDto)
    {
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
        var result = await _service.PatchAsync(id, patchDoc);

        if (!result) return NotFound();
        return Ok();
    }
}