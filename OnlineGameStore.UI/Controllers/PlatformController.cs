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

            return Created($"api/platforms/{createdPlatform.Id}", createdPlatform);
        }
        catch (ValidationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    /// <summary>
    /// Retrieves the list of platforms using pagination.
    /// </summary>
    /// <param name="pagingParams"> Specifies the pageSize and page pagination parameters.</param>
    [ProducesResponseType(typeof(PaginatedResponse<PlatformDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] PagingParams pagingParams)
    {
        var paginatedResponse = await _service.GetAsync(pagingParams: pagingParams);

        return Ok(paginatedResponse);
    }

    /// <summary>
    /// Retrieves a platform by its unique ID.
    /// </summary>
    /// <param name="id">The id of the platform to retrieve.</param>
    [ProducesResponseType(typeof(PlatformDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var platform = await _service.GetByIdAsync(id);

        return (platform is null) ? NotFound() : Ok(platform);
    }
}