using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.UI.DTOs.Platform;

namespace OnlineGameStore.UI.Controllers;


[Route("platforms")]
[ApiController]
public class PlatformController : ControllerBase
{
    private readonly IPlatformService _platformService;
    private readonly IMapper _mapper;
    
    public PlatformController(IPlatformService platformService, IMapper mapper)
    {
        _platformService = platformService;
        _mapper = mapper;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<PlatformResponseDto>> GetByIdAsync(Guid id)
    {
        var platformDto = await _platformService.GetByIdAsync(id);
        
        if (platformDto == null) return NotFound();
        
        var response = _mapper.Map<PlatformResponseDto>(platformDto);
        return Ok(response);
    }
    
    // enhance in future with filter, include and sort parameters
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlatformResponseDto>>> GetAllAsync()
    {
        var platformDtos = await _platformService.GetAllAsync();
        var responses = _mapper.Map<IEnumerable<PlatformResponseDto>>(platformDtos);
        return Ok(responses);
    }
    
    [HttpPost]
    public async Task<ActionResult<PlatformResponseDto>> CreateAsync([FromBody] PlatformRequestDto requestPlatformDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var existingPlatform = (await _platformService.GetAllAsync(
            filter: p => p.Name.Equals(requestPlatformDto.Name.Trim(), StringComparison.OrdinalIgnoreCase)
        )).FirstOrDefault();

        if (existingPlatform != null)
        {
            return Conflict($"Platform with name '{requestPlatformDto.Name}' already exists.");
        }
        
        var platformDto = _mapper.Map<PlatformDto>(requestPlatformDto);
        var createdDto = await _platformService.AddAsync(platformDto);

        if (createdDto == null)
        {
            return StatusCode(500, "An error occurred while creating the platform.");
        }
        
        var response = _mapper.Map<PlatformResponseDto>(createdDto);
        
        return Created($"/games/{response.Id}", response);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] PlatformRequestDto requestPlatformDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var existingPlatform = await _platformService.GetByIdAsync(id);
        
        if (existingPlatform == null) return NotFound();
        
        var nameExists = (await _platformService.GetAllAsync(
            filter: p => p.Name.Equals(requestPlatformDto.Name.Trim(), StringComparison.OrdinalIgnoreCase)
                         && !p.Id.Equals(id)
        )).Any();
       
        if (nameExists)
        {
            return Conflict($"Platform with name '{requestPlatformDto.Name}' already exists.");
        }
        
        var platformDto = _mapper.Map<PlatformDto>(requestPlatformDto);
        platformDto.Id = id;
        
        var result = await _platformService.UpdateAsync(platformDto);

        if (!result) return StatusCode(500, "An error occurred while creating the platform.");

        var updatedPlatform = await _platformService.GetByIdAsync(id);
        var response = _mapper.Map<PlatformResponseDto>(updatedPlatform);

        return Ok(response);
    }

    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var result = await _platformService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
    
    
}