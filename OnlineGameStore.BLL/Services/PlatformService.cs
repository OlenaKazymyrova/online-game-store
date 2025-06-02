using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.BLL.Services;

public class PlatformService : Service<Platform, PlatformDto>, IPlatformService
{
    public PlatformService(IPlatformRepository repository, IMapper mapper) 
        : base(repository, mapper) {}
    
}