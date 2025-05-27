using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;


namespace OnlineGameStore.UI.Controllers;


[Route("/licenses")]
[ApiController]
public class LicenseController : ControllerBase
{
    private readonly ILicenseService _licenseService;
    public LicenseController(ILicenseService licenseService)
    {
        _licenseService = licenseService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LicenseResponseDto>>> GetAll()
    {
        try
        {
            var licenses = await _licenseService.GetAllAsync();
            return Ok(licenses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<LicenseResponseDto>> GetLicenseById(Guid id)
    {
        try
        {
            var license = await _licenseService.GetByIdAsync(id);
            return Ok(license);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<LicenseResponseDto>> CreateLicense([FromBody] LicenseDto licenseDto)
    {
        try
        {
            LicenseResponseDto createdLicense = await _licenseService.CreateAsync(licenseDto);
            return CreatedAtAction(nameof(GetLicenseById), new { id = createdLicense.Id }, createdLicense);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLicense(Guid id, [FromBody] LicenseDto licenseDto)
    {
        try
        {
            await _licenseService.UpdateAsync(id, licenseDto);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }


    [HttpPatch("{id}")]
    public async Task<IActionResult> PartialUpdateLicense(Guid id, [FromBody] JsonPatchDocument<LicenseDto> patchDoc)
    {
        try
        {

            if (patchDoc == null)
            {
                return BadRequest("Patch document cannot be null");
            }

            var updatedLicense = await _licenseService.PatchAsync(id, patchDoc);

            return Ok(updatedLicense);
        }

        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while updating the license");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLicense(Guid id)
    {
        try
        {
            await _licenseService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }
}