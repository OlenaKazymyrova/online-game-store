using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Services;

public class UserRoleService : IUserRoleService
{
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public UserRoleService(IUserRoleRepository userRoleRepository, IUserRepository userRepository,
        IRoleRepository roleRepository, IMapper mapper)
    {
        _userRoleRepository = userRoleRepository;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoleReadDto>> GetUserRolesAsync(Guid userId)
    {
        var userRoles = await _userRoleRepository.GetUserRolesAsync(userId);
        return _mapper.Map<IEnumerable<RoleReadDto>>(userRoles);
    }
    
    public async Task<IEnumerable<UserReadDto>> GetUsersByRoleAsync(Guid roleId)
    {
        var users = await _userRoleRepository.GetUsersByRoleAsync(roleId);
        return _mapper.Map<IEnumerable<UserReadDto>>(users);
    }

    public async Task<bool> UserHasRoleAsync(Guid userId, Guid roleId)
    {
        CheckGuids(userId, roleId);
        await CheckEntityExistence(userId, roleId);

        return await _userRoleRepository.UserHasRoleAsync(userId, roleId);
    }

    public async Task<bool> AddUserRoleAsync(Guid userId, Guid roleId)
    {
        CheckGuids(userId, roleId);
        await CheckEntityExistence(userId, roleId);

        return await _userRoleRepository.AddUserRoleAsync(userId, roleId);
    }

    public async Task<bool> RemoveUserRoleAsync(Guid userId, Guid roleId)
    {
        CheckGuids(userId, roleId);
        await CheckEntityExistence(userId, roleId);

        return await _userRoleRepository.RemoveUserRoleAsync(userId, roleId);
    }

    private void CheckGuids(Guid userId, Guid roleId)
    {
        if (userId == Guid.Empty || roleId == Guid.Empty)
            throw new ArgumentException("User ID and Role ID must be valid GUIDs.");

        if (userId == roleId)
            throw new ArgumentException("User ID and Role ID cannot be the same.");
    }

    private async Task CheckEntityExistence(Guid userId, Guid roleId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {userId} not found.");

        var role = await _roleRepository.GetByIdAsync(roleId);
        if (role == null)
            throw new KeyNotFoundException($"Role with ID {roleId} not found.");
    }
}