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
    /// <param name="platformDto">The platform data to create.</param>
    [HttpPost]
    [ProducesResponseType(typeof(PlatformDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] PlatformCreateDto? platformDto)
    {
        if (platformDto == null)
        {
            return BadRequest("Platform data is required.");
        }

        try
        {
            var createdPlatform = await _service.AddAsync(platformDto);

            if (createdPlatform is null)
                return BadRequest("Failed to create platform.");

            return Created($"api/Platforms/{createdPlatform.Id}", createdPlatform);
        }
        catch (ValidationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    /// <summary>
    /// Updates all Platform fields
    /// </summary>
    /// <param name="id">The unique identifier of the Platform to update.</param>
    /// <param name="platformDto">The new Platform data.</param>
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePut([FromRoute] Guid id, [FromBody] PlatformCreateDto platformDto)
    {
        if (platformDto is null)
        {
            return BadRequest("Platform data is required.");
        }

        try
        {
            var isUpdated = await _service.UpdateAsync(id, platformDto);

            return (isUpdated) ? Ok() : NotFound();
        }
        catch (ValidationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}