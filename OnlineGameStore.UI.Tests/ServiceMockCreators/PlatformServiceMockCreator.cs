using System.ComponentModel.DataAnnotations;
using Moq;
using OnlineGameStore.BLL.DTOs;
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

    protected override void SetupUpdate(Mock<IPlatformService> mock)
    {
        mock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PlatformCreateDto>()))
            .ReturnsAsync((Guid id, PlatformCreateDto updateDto) =>
            {
                int index = _data.FindIndex(p => p.Id == id);
                if (index == -1)
                {
                    return false;
                }

                if (_data.Any(p =>
                        p.Name.Equals(updateDto.Name, StringComparison.OrdinalIgnoreCase)
                        && p.Id != id))
                {
                    throw new ValidationException("Platform name already exists.");
                }

                var entity = _mapper.Map<Platform>(updateDto);
                entity.Id = id;
                _data[index] = entity;

                return true;
            });
    }
}