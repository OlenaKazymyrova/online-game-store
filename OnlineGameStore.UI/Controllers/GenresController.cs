using Microsoft.AspNetCore.Mvc;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;

namespace OnlineGameStore.UI.Controllers;


[Route("api/[Controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly IGenreService _service;

    public GenresController(IGenreService service)
    {
        _service = service;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var dto = await _service.GetByIdAsync(id);

        return (dto is null) ? NotFound() : Ok(dto);
    }

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
        var genres = await _service.GetAllAsync();
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
}
