using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Interfaces;

public interface IPlatformService : IService<Platform, PlatformCreateDto, PlatformDto, PlatformDto> { }