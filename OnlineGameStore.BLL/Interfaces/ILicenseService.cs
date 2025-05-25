using OnlineGameStore.BLL.DTOs;

namespace OnlineGameStore.BLL.Interfaces;

public interface ILicenseService
{
    Task<LicenseResponseDto> GetByGameIdAsync(Guid gameId);
    Task<IEnumerable<LicenseResponseDto>> GetAllAsync();
    Task<LicenseResponseDto> GetByIdAsync(Guid id);
    Task<LicenseResponseDto> CreateAsync(Guid gameId, LicenseDto licenseCreateDto);
    Task UpdateAsync(Guid id, LicenseDto licenseUpdateDto);
    Task DeleteAsync(Guid id);
    

}