using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.SharedLogic.Pagination;

namespace OnlineGameStore.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformService _service;

    public PlatformsController(IPlatformService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Creates a new platform.
    /// </summary>
    /// <param name="dto">The platform data to create.</param>
    [HttpPost]
    [ProducesResponseType(typeof(PlatformDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] PlatformCreateDto dto)
    {
        try
        {
            var createdPlatform = await _service.AddAsync(dto);

            if (createdPlatform is null)
                return BadRequest("Failed to create platform.");

            return Created($"api/Platforms/{createdPlatform.Id}", createdPlatform);
        }
        catch (ValidationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}