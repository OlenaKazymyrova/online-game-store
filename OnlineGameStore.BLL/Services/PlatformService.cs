using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Interfaces;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.BLL.Services;

public class PlatformService : Service<Platform, PlatformCreateDto, PlatformDto, PlatformDto>, IPlatformService
{
    public PlatformService(IPlatformRepository repository, IMapper mapper)
        : base(repository, mapper) { }

}