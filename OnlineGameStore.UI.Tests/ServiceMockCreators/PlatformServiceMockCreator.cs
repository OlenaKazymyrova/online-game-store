using System.ComponentModel.DataAnnotations;
using Moq;
using OnlineGameStore.BLL.DTOs.Platforms;
using OnlineGameStore.BLL.Interfaces;

namespace OnlineGameStore.UI.Tests.ServiceMockCreators;

public class PlatformServiceMockCreator
    : ServiceMockCreator<Platform, PlatformCreateDto, PlatformDto, PlatformDto, IPlatformService>
{
    public PlatformServiceMockCreator(List<Platform> data) : base(data)
    {
    }

    protected override void SetupAdd(Mock<IPlatformService> mock)
    {
        mock.Setup(x => x.AddAsync(It.IsAny<PlatformCreateDto>()))
            .ReturnsAsync((PlatformCreateDto createDto) =>
            {
                if (_data.Any(p => p.Name.Equals(createDto.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ValidationException("Platform name already exists.");
                }

                var entity = _mapper.Map<Platform>(createDto);
                entity.Id = Guid.NewGuid();
                _data.Add(entity);
                return _mapper.Map<PlatformDto>(entity);
            });
    }
}