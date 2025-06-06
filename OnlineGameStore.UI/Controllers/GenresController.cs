﻿using Microsoft.AspNetCore.Mvc;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;

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
    /// Retrieves the list of all genres.
    /// </summary>
    [ProducesResponseType(typeof(List<GenreReadDto>), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var genres = await _service.GetAsync();
        return (genres is null) ? Ok(new List<GenreReadDto>()) : Ok(genres);
    }

    /// <summary>
    /// Creates a new genre.
    /// </summary>
    /// <param name="dto">The genre data to create from.</param>
    [ProducesResponseType(typeof(GenreDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GenreDto dto)
    {
        if (dto is null)
        {
            return BadRequest("Genre data is invalid or not present");
        }

        if (dto.Id is null
            || dto.Id == Guid.Empty)
        {
            dto.Id = Guid.NewGuid();
        }

        // ###############
        // this is to be handled with filters in the future
        var genres = await _service.GetAsync();
        if (genres.Any(existing => existing.Equals(dto)))
        {
            return Conflict("The genre already exists");
        }
        // ###############

        var createdGenre = await _service.AddAsync(dto);

        if (createdGenre is null)
        {
            return StatusCode(500);
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdGenre.Id },
            createdGenre
        );
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