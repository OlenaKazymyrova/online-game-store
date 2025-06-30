using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineGameStore.BLL.Authentication.Interface;
using OnlineGameStore.BLL.DTOs.Logins;
using OnlineGameStore.BLL.DTOs.Tokens;
using OnlineGameStore.BLL.DTOs.Users;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.SharedLogic.Constants;

namespace OnlineGameStore.BLL.Services;

public class UserService : Service<User, UserCreateDto, UserReadDto, UserCreateDto, UserReadDto>, IUserService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;

    public UserService(IUserRepository repository, IMapper mapper, IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
        : base(repository, mapper)
    {
        _passwordHasher = passwordHasher;
        _userRepository = repository;
        _jwtProvider = jwtProvider;
    }

    public override async Task<UserReadDto?> AddAsync(UserCreateDto? dto)
    {
        if (dto == null)
        {
            return null;
        }

        bool userExists = await _userRepository.GetByNameAsync(dto.Username) != null;
        if (userExists)
        {
            throw new ArgumentException("Invalid argument", nameof(dto.Username));
        }

        userExists = await _userRepository.GetByEmailAsync(dto.Email) != null;
        if (userExists)
        {
            throw new ArgumentException("Invalid argument", nameof(dto.Email));
        }

        if (dto.Password.Length < UserConstants.PasswordMinLength)
        {
            throw new ArgumentException(
                $"Password must be at least {UserConstants.PasswordMinLength} characters long", nameof(dto.Password));
        }

        try
        {
            var hashedPassword = _passwordHasher.Generate(dto.Password);

            var user = _mapper.Map<User>(dto);
            user.PasswordHash = hashedPassword;

            var createdUser = await _repository.AddAsync(user);
            return _mapper.Map<UserReadDto>(createdUser);
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error adding user: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return null;
        }
    }

    public async Task<TokenResponseDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);
        if (user is null)
        {
            return null;
        }

        var isValid = _passwordHasher.Verify(loginDto.Password, user.PasswordHash);

        if (!isValid)
        {
            return null;
        }

        var accessToken = _jwtProvider.GenerateAccessToken(user);
        var refreshToken = _jwtProvider.GenerateRefreshToken();
        var refreshExpiry = DateTime.UtcNow.AddDays(7);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = refreshExpiry;

        await _userRepository.UpdateAsync(user);

        return new TokenResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            RefreshTokenExpiry = refreshExpiry
        };
    }

    public async Task<TokenResponseDto?> RefreshTokenAsync(string refreshToken)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);

        if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            return null;

        var newAccessToken = _jwtProvider.GenerateAccessToken(user);
        var newRefreshToken = _jwtProvider.GenerateRefreshToken();
        var newExpiry = DateTime.UtcNow.AddDays(7);

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = newExpiry;

        await _userRepository.UpdateAsync(user);

        return new TokenResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            RefreshTokenExpiry = newExpiry
        };
    }
}