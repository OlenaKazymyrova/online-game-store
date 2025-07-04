﻿using Microsoft.AspNetCore.Authorization;
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
    [HttpGet("{id:guid}")]
    [Authorize(Policy = "Permissions.Read")]
    [ProducesResponseType(typeof(GenreDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var dto = await _service.GetByIdAsync(id);
        return Ok(dto);
    }

    /// <summary>
    /// Retrieves the list of genres using pagination.
    /// </summary>
    /// <param name="aggregationParams"> Specifies the possible filtering and logic.</param>
    /// <param name="pagingParams"> Specifies the pageSize and page pagination parameters.</param>
    [HttpGet]
    [Authorize(Policy = "Permissions.Read")]
    [ProducesResponseType(typeof(PaginatedResponse<GenreDetailedDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
    [HttpPost]
    [Authorize(Policy = "Permissions.Create")]
    [ProducesResponseType(typeof(GenreDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] GenreCreateDto dto)
    {
        var createdGenre = await _service.AddAsync(dto);
        return CreatedAtAction(
            nameof(GetById),
            new { id = createdGenre.Id },
            createdGenre
        );
    }

    /// <summary>
    /// Updates the list of referenced games for a specified genre.
    /// </summary>
    /// <param name="id"> The id of the genre to update.</param>>
    /// <param name="gameIds"> The update list of Games Ids.</param>>
    [HttpPut("{id:guid}/games")]
    [Authorize(Policy = "Permissions.Update")]
    public async Task<IActionResult> UpdateGames(Guid id, List<Guid> gameIds)
    {
        await _service.UpdateGameRefsAsync(id, gameIds);
        return Ok();
    }

    /// <summary>
    /// Updates all Genre fields
    /// </summary>
    /// <param name="id">The unique identifier of the Genre to update.</param>
    /// <param name="genreDto">The new Genre data.</param>
    [HttpPut("{id:guid}")]
    [Authorize(Policy = "Permissions.Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePut([FromRoute] Guid id, [FromBody] GenreCreateDto genreDto)
    {
        var isUpdated = await _service.UpdateAsync(id, genreDto);
        return isUpdated ? Ok() : NotFound();
    }

    /// <summary>
    /// Deletes a genre by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the genre to delete.</param>
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "Permissions.Delete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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