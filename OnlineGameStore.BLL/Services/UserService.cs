using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineGameStore.BLL.DTOs.Users;
using OnlineGameStore.BLL.Exceptions;
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

    public override async Task<UserReadDto> AddAsync(UserCreateDto dto)
    {
        if (dto == null)
            throw new ValidationException("UserCreateDto is required for create.");

        var user = TryMap(dto);

        if (user == null)
            throw new ValidationException("Failed to map UserCreateDto to User entity.");

        try
        {
            var responseDto = await _repository.AddAsync(user);
            return _mapper.Map<UserReadDto>(responseDto);
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
}