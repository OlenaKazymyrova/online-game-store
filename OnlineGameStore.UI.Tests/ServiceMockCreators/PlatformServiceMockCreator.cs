using AutoMapper;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.UI.Tests.ServiceMockCreators;

public class PlatformServiceMockCreator: ServiceMockCreator<Platform,PlatformDto, IPlatformService>
{
    public PlatformServiceMockCreator(List<PlatformDto> data ) : base(data) {}
}