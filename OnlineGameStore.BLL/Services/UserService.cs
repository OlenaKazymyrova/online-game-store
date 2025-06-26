using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.DTOs.Users;
using OnlineGameStore.BLL.Exceptions;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.SharedLogic.Constants;

namespace OnlineGameStore.BLL.Services;

public class UserService : Service<User, UserCreateDto, UserReadDto, UserCreateDto, UserReadDto>, IUserService
{
    public UserService(IUserRepository repository, IMapper mapper)
        : base(repository, mapper)
    {
    }

    public override async Task<UserReadDto> AddAsync(UserCreateDto dto)
    {
        if (dto == null)
            throw new ValidationException("UserCreateDto is required for create.");

        var user = _mapper.Map<User>(dto);

        if (user == null)
            throw new ValidationException("Failed to map UserCreateDto to User entity.");

        try
        {
            var responseDto = await _repository.AddAsync(user);
            return _mapper.Map<UserReadDto>(responseDto);
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error adding user: {ex.Message}");
            throw new ConflictException("Failed to add user. Please check the data and try again.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            throw new InternalErrorException(
                "An unexpected error occurred while adding the user. Please try again later.");
        }
    }
}