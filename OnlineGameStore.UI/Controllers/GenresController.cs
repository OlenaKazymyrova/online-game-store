using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineGameStore.BLL.DTOs.Genres;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.SharedLogic.Pagination;
using OnlineGameStore.UI.Aggregation;
using OnlineGameStore.UI.QueryBuilders;

namespace OnlineGameStore.UI.Controllers;

[Route("api/[Controller]")]
[ApiController]
[Authorize]
public class GenresController : ControllerBase
{
    public GenresController(IGenreService service)
    {
        _service = service;
    }

    private readonly IGenreService _service;

    /// <summary>
    /// Retrieves a genre by its unique ID.
    /// </summary>
    /// <param name="id">The id of the genre to retrieve.</param>
    [ProducesResponseType(typeof(GenreDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var dto = await _service.GetByIdAsync(id);

        return (dto is null) ? NotFound() : Ok(dto);
    }

    /// <summary>
    /// Retrieves the list of genres using pagination.
    /// </summary>
    /// <param name="aggregationParams"> Specifies the possible filtering and logic.</param>
    /// <param name="pagingParams"> Specifies the pageSize and page pagination parameters.</param>
    [ProducesResponseType(typeof(PaginatedResponse<GenreDetailedDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GenreAggregationParams aggregationParams, [FromQuery] PagingParams pagingParams)
    {
        var queryBuilder = new GenreQueryBuilder();

        var filter = queryBuilder.BuildFilter(aggregationParams);
        var include = queryBuilder.BuildInclude(aggregationParams);

        var paginatedResponse = await _service.GetAsync(
            filter: filter,
            include: include,
            pagingParams: pagingParams);

        return Ok(paginatedResponse);
    }

    /// <summary>
    /// Creates a new genre.
    /// </summary>
    /// <param name="dto">The genre data to create from.</param>
    [ProducesResponseType(typeof(GenreDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GenreCreateDto dto)
    {
        if (dto is null)
        {
            return BadRequest("Genre data is invalid or not present");
        }

        var createdGenre = await _service.AddAsync(dto);

        if (createdGenre is null)
        {
            return BadRequest();
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdGenre.Id },
            createdGenre
        );
    }


    /// <summary>
    /// Updates all Genre fields
    /// </summary>
    /// <param name="id">The unique identifier of the Genre to update.</param>
    /// <param name="genreDto">The new Genre data.</param>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePut([FromRoute] Guid id, [FromBody] GenreCreateDto genreDto)
    {
        if (genreDto is null)
        {
            return BadRequest("Genre data is required");
        }

        var isUpdated = await _service.UpdateAsync(id, genreDto);

        return (isUpdated) ? Ok() : NotFound();
    }

    /// <summary>
    /// Deletes a genre by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the genre to delete.</param>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var isDeleted = await _service.DeleteAsync(id);

        if (!isDeleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}