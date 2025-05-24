using Microsoft.AspNetCore.Mvc;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.UI.Controllers;

[Route("/games")]
public class GameController : ControllerBase
{
    private readonly IGameRepository _repository;

    public GameController(IGameRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var game = await _repository.GetByIdAsync(id);
        if (game == null)
        {
            return NotFound();
        }

        return Ok(game);
    }
}