using Microsoft.AspNetCore.JsonPatch;
using OnlineGameStore.BLL.DTOs;

namespace OnlineGameStore.BLL.Interfaces;

public interface ILicenseService
{
   
    Task<IEnumerable<LicenseResponseDto>> GetAllAsync();
    Task<LicenseResponseDto> GetByIdAsync(Guid id);
    Task<LicenseResponseDto> CreateAsync(LicenseDto licenseCreateDto);
    Task UpdateAsync(Guid id, LicenseDto licenseUpdateDto);
    Task DeleteAsync(Guid id);
    Task<LicenseResponseDto> PatchAsync(Guid id, JsonPatchDocument<LicenseDto> patchDoc);
    

}