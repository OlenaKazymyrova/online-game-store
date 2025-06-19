using Microsoft.AspNetCore.Mvc;
using OnlineGameStore.BLL.DTOs.Genres;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.SharedLogic.Pagination;

namespace OnlineGameStore.UI.Controllers;

[Route("api/[Controller]")]
[ApiController]
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
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var dto = await _service.GetByIdAsync(id);

        return (dto is null) ? NotFound() : Ok(dto);
    }

    /// <summary>
    /// Retrieves the list of genres using pagination.
    /// </summary>
    /// <param name="pagingParams"> Specifies the pageSize and page pagination parameters.</param>
    [ProducesResponseType(typeof(List<GenreReadDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PagingParams pagingParams)
    {
        var paginatedResponse = await _service.GetAsync(pagingParams: pagingParams);

        return (paginatedResponse is null) ? StatusCode(500) : Ok(paginatedResponse);
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
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
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
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
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