using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Services;

public class LicenseService: ILicenseService
{
    private readonly ILicenseRepository _licenseRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IMapper _mapper;

    public LicenseService(ILicenseRepository licenseRepository, IGameRepository gameRepository, IMapper mapper)
    {
        _licenseRepository = licenseRepository;
        _gameRepository = gameRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LicenseResponseDto>> GetAllAsync()
    {
        IEnumerable<License> licenses = await _licenseRepository.GetAsync();
        return _mapper.Map<IEnumerable<LicenseResponseDto>>(licenses);

    }

    public async Task<LicenseResponseDto> GetByIdAsync(Guid id)
    {
        License license = await _licenseRepository.GetByIdAsync(id);
        if (license == null)
        {
            throw new KeyNotFoundException($"License with {id} was not found");
        }
        return _mapper.Map<LicenseResponseDto>(license);
    }
    

    public async Task<LicenseResponseDto> CreateAsync( LicenseDto licenseCreateDto)
    {
        if (licenseCreateDto == null)
        {
            throw new ValidationException("No License data is provided");
        }

        
        bool gameExists = await _gameRepository.GetAsync(filter: g => g.Id == licenseCreateDto.GameId) != null ;
        
        if (!gameExists)
        {
            throw new KeyNotFoundException($"Game with {licenseCreateDto.GameId} was not found");
        }
        
        bool hasLicense = await _licenseRepository.GetAsync(filter: l => l.GameId == licenseCreateDto.GameId) != null ;

        if (hasLicense)
        {
            throw new ArgumentException($"Game already has a licence");
        }

        License license = _mapper.Map<License>(licenseCreateDto);
        license.Id = Guid.NewGuid();
        license.GameId = licenseCreateDto.GameId;

        var createdLicense = await _licenseRepository.AddAsync(license);
        return _mapper.Map<LicenseResponseDto>(createdLicense);

    }

    public async Task UpdateAsync(Guid id, LicenseDto licenceUpdateDto)
    {
        License? existingLicense = await _licenseRepository.GetByIdAsync(id);
        if (existingLicense == null)
        {
            throw new KeyNotFoundException($"License with {id} was not found");
        }

        License updatedLicense = _mapper.Map<License>(licenceUpdateDto);
        updatedLicense.Id = id;
        await _licenseRepository.UpdateAsync(updatedLicense);
    }

    public async Task<LicenseResponseDto> PatchAsync(Guid id, JsonPatchDocument<LicenseDto> patchDoc)
    {

        var existingLicense = await _licenseRepository.GetByIdAsync(id);
        
        if (existingLicense == null)
        {
            throw new KeyNotFoundException($"License with id {id} not found");
        }

        LicenseDto licenseToPatch = _mapper.Map<LicenseDto>(existingLicense);
        patchDoc.ApplyTo(licenseToPatch);

        
        if (licenseToPatch.GameId != existingLicense.GameId)
        {
            bool gameExists = await _gameRepository.GetAsync(filter: g => g.Id == licenseToPatch.GameId)!= null ;
        
            if (!gameExists)
            {
                throw new KeyNotFoundException($"Game with {licenseToPatch.GameId} was not found");
            }
        
            bool hasLicense = await _licenseRepository.GetAsync(filter: l => l.GameId == licenseToPatch.GameId) != null ;

            if (hasLicense)
            {
                throw new ArgumentException("Game already has a licence");
            }

           
        }

        License updatedLicense = _mapper.Map<License>(licenseToPatch);
        await _licenseRepository.UpdateAsync(updatedLicense);
        
        return _mapper.Map<LicenseResponseDto>(updatedLicense);
    }

    public async Task DeleteAsync(Guid id)
    {
        License? license = await _licenseRepository.GetByIdAsync(id);
        if (license == null)
        {
            throw new KeyNotFoundException($"License {id} not found");
        }

        await _licenseRepository.DeleteByIdAsync(id);
    }
    
    
    
}