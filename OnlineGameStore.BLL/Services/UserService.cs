using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineGameStore.SharedLogic.Constants;
using OnlineGameStore.BLL.Authentication.Interface;
using OnlineGameStore.BLL.DTOs.Logins;
using OnlineGameStore.BLL.DTOs.Tokens;
using OnlineGameStore.BLL.DTOs.Users;
using OnlineGameStore.BLL.Exceptions;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

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
            throw new ConflictException("Username already exists.");
        }

        userExists = await _userRepository.GetByEmailAsync(dto.Email) != null;
        if (userExists)
        {
            throw new ConflictException("An account with this email already exists.");
        }

        if (dto.Password.Length < UserConstants.PasswordMinLength)
        {
            throw new ValidationException(
                $"Password must be at least {UserConstants.PasswordMinLength} characters long");
        }

        try
        {
            var hashedPassword = _passwordHasher.Generate(dto.Password);

            var user = _mapper.Map<User>(dto);
            user.PasswordHash = hashedPassword;

            var createdUser = await _repository.AddAsync(user);
            return _mapper.Map<UserReadDto>(createdUser);
        }
        catch (ArgumentNullException ex)
        {
            throw new ValidationException("UserCreateDto cannot be null. Please provide valid user data.", ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ConflictException("Failed to add user. Please check the data and try again.", ex);
        }
        catch (Exception ex)
        {
            throw new InternalErrorException(
                "An unexpected error occurred while adding the user. Please try again later.", ex);
        }
    }

    public async Task<TokenResponseDto?> LoginAsync(LoginDto? loginDto)
    {
        if (loginDto is null)
            throw new ValidationException("LoginDto is required for create.");

        var user = await _userRepository.GetByEmailAsync(loginDto.Email);

        var isValid = user != null ? _passwordHasher.Verify(loginDto.Password, user.PasswordHash) : false;
        if (!isValid)
        {
            throw new UnauthorizedException("Invalid email or password.");
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

    public async Task<TokenResponseDto?> RefreshTokenAsync(string? refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            throw new ValidationException("Refresh token must be provided.");
        }
        
        var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);

        if (user == null)
        {
            throw new UnauthorizedException("Invalid refresh token.");
        }

        if (user.RefreshTokenExpiryTime < DateTime.UtcNow)
        {
            throw new UnauthorizedException("Refresh token has expired.");
        }

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