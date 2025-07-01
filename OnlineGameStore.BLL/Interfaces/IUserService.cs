using OnlineGameStore.BLL.DTOs.Logins;
using OnlineGameStore.BLL.DTOs.Tokens;
using OnlineGameStore.BLL.DTOs.Users;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.SharedLogic.Enums;

namespace OnlineGameStore.BLL.Interfaces;

public interface IUserService : IService<User, UserCreateDto, UserReadDto, UserCreateDto, UserReadDto>
{
    Task<TokenResponseDto?> LoginAsync(LoginDto loginDto);

    Task<TokenResponseDto?> RefreshTokenAsync(string refreshToken);

}