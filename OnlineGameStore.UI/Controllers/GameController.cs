using Microsoft.AspNetCore.Mvc;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;

namespace OnlineGameStore.UI.Controllers;

[Route("games")]
public class GameController : ControllerBase
{
    private readonly IGameService _service;

    public GameController(IGameService service)
    {
        _service = service;
    }

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

        return CreatedAtAction("Create", new { id = createdGame.Id }, createdGame);
    }
}