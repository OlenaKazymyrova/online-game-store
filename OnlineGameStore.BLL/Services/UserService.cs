using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.DTOs.Users;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Services;

public class UserService : Service<User, UserCreateDto, UserReadDto, UserCreateDto, UserReadDto>, IUserService
{
    public UserService(IUserRepository repository, IMapper mapper)
        : base(repository, mapper)
    {
    }

    public override async Task<UserReadDto?> AddAsync(UserCreateDto dto)
    {
        var user = _mapper.Map<User>(dto);

        if (user == null)
        {
            return null;
        }

        if (user.PasswordHash.Length < 8)
        {
            return null;
        }

        try
        {
            var responseDto = await _repository.AddAsync(user);
            return _mapper.Map<UserReadDto>(responseDto);
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
}