using Microsoft.AspNetCore.Mvc;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;

namespace OnlineGameStore.UI.Controllers;

[Route("/platforms")]
public class PlatformController : ControllerBase
{
    private readonly IPlatformService _platformService;

    public PlatformController(IPlatformService platformService)
    {
        _platformService = platformService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlatformResponseDto>>> GetAllPlatforms()
    {
        try
        {
            IEnumerable<PlatformResponseDto> platforms = await _platformService.GetAllAsync();
            return Ok(platforms);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal error.");
        }
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<PlatformResponseDto>> GetPlatformById(Guid id)
    {
        try
        {
            PlatformResponseDto platform = await _platformService.GetByIdAsync(id);
            return Ok(platform);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal error.");
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<PlatformResponseDto>> CreatePlatform([FromBody] PlatformDto platformDto)
    {
        try
        {
            PlatformResponseDto createdPlatform = await _platformService.CreateAsync(platformDto);
            return CreatedAtAction(nameof(GetPlatformById), new { id = createdPlatform.Id }, createdPlatform);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal error.");
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlatform(Guid id, [FromBody] PlatformDto platformDto)
    {
        try
        {
            await _platformService.UpdateAsync(id, platformDto);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal error.");
        }
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchPlatform(Guid id, [FromBody] JsonPatchDocument<PlatformDto> patchDocument)
    {
        try
        {
            await _platformService.PatchAsync(id, patchDocument);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlatform(Guid id)
    {
        try
        {
            await _platformService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal error.");
        }
    }
    
    [HttpPost("{id}/games")]
    public async Task<IActionResult> AddGamesToPlatform(Guid id, [FromBody] List<Guid> gameIds)
    {
        try
        {
            await _platformService.AddGamesToPlatform(id, gameIds);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    
    [HttpDelete("{id}/games")]
    public async Task<IActionResult> RemoveGamesFromPlatform(Guid id, [FromBody] List<Guid> gameIds)
    {
        try
        {
            await _platformService.RemoveGamesFromPlatform(id, gameIds);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
           
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    
    [HttpPut("{id}/games")]
    public async Task<IActionResult> ReplaceGamesInPlatform(Guid id, [FromBody] List<Guid> gameIds)
    {
        try
        {
            await _platformService.ReplaceGamesInPlatform(id, gameIds);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}