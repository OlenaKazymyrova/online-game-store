using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.UI.Controllers;

public class PlatformControllerTests
{
    private readonly Mock<IPlatformService> _platformServiceMock;
    private readonly PlatformController _controller;

    public PlatformControllerTests()
    {
        _platformServiceMock = new Mock<IPlatformService>();
        _controller = new PlatformController(_platformServiceMock.Object);
    }

    [Fact]
    public async Task GetAllPlatforms_ReturnsOkResult_WithPlatforms()
    {

        var platforms = new List<PlatformResponseDto>
        {
            new PlatformResponseDto { Id = Guid.NewGuid(), Name = "Platform 1" },
            new PlatformResponseDto { Id = Guid.NewGuid(), Name = "Platform 2" }
        };
        _platformServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(platforms);

        var result = await _controller.GetAllPlatforms();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnPlatforms = Assert.IsAssignableFrom<IEnumerable<PlatformResponseDto>>(okResult.Value);
        Assert.Equal(2, ((List<PlatformResponseDto>)returnPlatforms).Count);
    }

    [Fact]
    public async Task GetAllPlatforms_ReturnsInternalServerError_OnException()
    {
        _platformServiceMock.Setup(s => s.GetAllAsync()).ThrowsAsync(new Exception());

        var result = await _controller.GetAllPlatforms();

        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task GetPlatformById_ReturnsOk_WithPlatform()
    {
        var id = Guid.NewGuid();
        var platform = new PlatformResponseDto { Id = id, Name = "Test Platform" };
        _platformServiceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(platform);

        var result = await _controller.GetPlatformById(id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(platform, okResult.Value);
    }

    [Fact]
    public async Task GetPlatformById_ReturnsNotFound_WhenKeyNotFound()
    {
        var id = Guid.NewGuid();
        _platformServiceMock.Setup(s => s.GetByIdAsync(id)).ThrowsAsync(new KeyNotFoundException("Not found"));

        var result = await _controller.GetPlatformById(id);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("Not found", notFoundResult.Value);
    }

    [Fact]
    public async Task CreatePlatform_ReturnsCreatedAtAction_WithCreatedPlatform()
    {
        var platformDto = new PlatformDto { Name = "New Platform" };
        var createdPlatform = new PlatformResponseDto { Id = Guid.NewGuid(), Name = "New Platform" };

        _platformServiceMock.Setup(s => s.CreateAsync(platformDto)).ReturnsAsync(createdPlatform);

        var result = await _controller.CreatePlatform(platformDto);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(PlatformController.GetPlatformById), createdAtActionResult.ActionName);
        Assert.Equal(createdPlatform, createdAtActionResult.Value);
    }

    [Fact]
    public async Task CreatePlatform_ReturnsBadRequest_WhenKeyNotFoundException()
    {
        var platformDto = new PlatformDto { Name = "New Platform" };
        _platformServiceMock.Setup(s => s.CreateAsync(platformDto))
            .ThrowsAsync(new KeyNotFoundException("Bad request"));

        var result = await _controller.CreatePlatform(platformDto);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Bad request", badRequestResult.Value);
    }

    [Fact]
    public async Task UpdatePlatform_ReturnsNoContent_OnSuccess()
    {
        var id = Guid.NewGuid();
        var platformDto = new PlatformDto { Name = "Updated Platform" };

        _platformServiceMock.Setup(s => s.UpdateAsync(id, platformDto)).Returns(Task.CompletedTask);

        var result = await _controller.UpdatePlatform(id, platformDto);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdatePlatform_ReturnsNotFound_WhenKeyNotFoundException()
    {
        var id = Guid.NewGuid();
        var platformDto = new PlatformDto();
        _platformServiceMock.Setup(s => s.UpdateAsync(id, platformDto)).ThrowsAsync(new KeyNotFoundException("Not found"));

        var result = await _controller.UpdatePlatform(id, platformDto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Not found", notFoundResult.Value);
    }

    [Fact]
    public async Task PatchPlatform_ReturnsNoContent_OnSuccess()
    {
        var id = Guid.NewGuid();
        var patchDoc = new JsonPatchDocument<PlatformDto>();

        _platformServiceMock.Setup(s => s.PatchAsync(id, patchDoc)).Returns(Task.CompletedTask);

        var result = await _controller.PatchPlatform(id, patchDoc);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task PatchPlatform_ReturnsNotFound_WhenKeyNotFoundException()
    {
        var id = Guid.NewGuid();
        var patchDoc = new JsonPatchDocument<PlatformDto>();
        _platformServiceMock.Setup(s => s.PatchAsync(id, patchDoc)).ThrowsAsync(new KeyNotFoundException("Not found"));

        var result = await _controller.PatchPlatform(id, patchDoc);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Not found", notFoundResult.Value);
    }

    [Fact]
    public async Task PatchPlatform_ReturnsBadRequest_WhenArgumentException()
    {
        var id = Guid.NewGuid();
        var patchDoc = new JsonPatchDocument<PlatformDto>();
        _platformServiceMock.Setup(s => s.PatchAsync(id, patchDoc)).ThrowsAsync(new ArgumentException("Bad request"));

        var result = await _controller.PatchPlatform(id, patchDoc);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Bad request", badRequestResult.Value);
    }

    [Fact]
    public async Task DeletePlatform_ReturnsNoContent_OnSuccess()
    {
        var id = Guid.NewGuid();
        _platformServiceMock.Setup(s => s.DeleteAsync(id)).Returns(Task.CompletedTask);

        var result = await _controller.DeletePlatform(id);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeletePlatform_ReturnsInternalServerError_OnException()
    {
        var id = Guid.NewGuid();
        _platformServiceMock.Setup(s => s.DeleteAsync(id)).ThrowsAsync(new Exception());

        var result = await _controller.DeletePlatform(id);

        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task AddGamesToPlatform_ReturnsNoContent_OnSuccess()
    {
        var id = Guid.NewGuid();
        var gameIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        _platformServiceMock.Setup(s => s.AddGamesToPlatform(id, gameIds)).Returns(Task.CompletedTask);

        var result = await _controller.AddGamesToPlatform(id, gameIds);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task AddGamesToPlatform_ReturnsNotFound_WhenKeyNotFoundException()
    {
        var id = Guid.NewGuid();
        var gameIds = new List<Guid> { Guid.NewGuid() };
        _platformServiceMock.Setup(s => s.AddGamesToPlatform(id, gameIds)).ThrowsAsync(new KeyNotFoundException("Not found"));

        var result = await _controller.AddGamesToPlatform(id, gameIds);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Not found", notFoundResult.Value);
    }

    [Fact]
    public async Task RemoveGamesFromPlatform_ReturnsNoContent_OnSuccess()
    {
        var id = Guid.NewGuid();
        var gameIds = new List<Guid> { Guid.NewGuid() };
        _platformServiceMock.Setup(s => s.RemoveGamesFromPlatform(id, gameIds)).Returns(Task.CompletedTask);

        var result = await _controller.RemoveGamesFromPlatform(id, gameIds);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task RemoveGamesFromPlatform_ReturnsNotFound_WhenKeyNotFoundException()
    {
        var id = Guid.NewGuid();
        var gameIds = new List<Guid> { Guid.NewGuid() };
        _platformServiceMock.Setup(s => s.RemoveGamesFromPlatform(id, gameIds)).ThrowsAsync(new KeyNotFoundException("Not found"));

        var result = await _controller.RemoveGamesFromPlatform(id, gameIds);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Not found", notFoundResult.Value);
    }

    [Fact]
    public async Task ReplaceGamesInPlatform_ReturnsNoContent_OnSuccess()
    {
        var id = Guid.NewGuid();
        var gameIds = new List<Guid> { Guid.NewGuid() };
        _platformServiceMock.Setup(s => s.ReplaceGamesInPlatform(id, gameIds)).Returns(Task.CompletedTask);

        var result = await _controller.ReplaceGamesInPlatform(id, gameIds);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task ReplaceGamesInPlatform_ReturnsNotFound_WhenKeyNotFoundException()
    {
        var id = Guid.NewGuid();
        var gameIds = new List<Guid> { Guid.NewGuid() };
        _platformServiceMock.Setup(s => s.ReplaceGamesInPlatform(id, gameIds)).ThrowsAsync(new KeyNotFoundException("Not found"));

        var result = await _controller.ReplaceGamesInPlatform(id, gameIds);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Not found", notFoundResult.Value);
    }
}
