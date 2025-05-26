using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineGameStore.BLL.DTOs;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.UI.Controllers;
using Xunit;

namespace OnlineGameStore.Tests.Controllers
{
    public class LicenseControllerTests
    {
        private readonly Mock<ILicenseService> _licenseServiceMock;
        private readonly LicenseController _controller;

        public LicenseControllerTests()
        {
            _licenseServiceMock = new Mock<ILicenseService>();
            _controller = new LicenseController(_licenseServiceMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WithListOfLicenses()
        {
            var licenses = new List<LicenseResponseDto> 
            { 
                new LicenseResponseDto 
                { 
                    Id = Guid.NewGuid(), 
                    GameId = Guid.NewGuid(), 
                    Description = "Sample license", 
                    Cost = 99.99m 
                } 
            };
            _licenseServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(licenses);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedLicenses = Assert.IsAssignableFrom<IEnumerable<LicenseResponseDto>>(okResult.Value);
            Assert.Single(returnedLicenses);
        }

        [Fact]
        public async Task GetAll_ReturnsStatusCode500_OnException()
        {
            _licenseServiceMock.Setup(s => s.GetAllAsync()).ThrowsAsync(new Exception());

            var result = await _controller.GetAll();

            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Internal server error", statusCodeResult.Value);
        }

        [Fact]
        public async Task GetLicenseById_ReturnsOk_WithLicense()
        {
            var id = Guid.NewGuid();
            var license = new LicenseResponseDto 
            { 
                Id = id, 
                GameId = Guid.NewGuid(),
                Description = "Test License", 
                Cost = 49.99m 
            };
            _licenseServiceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(license);

            var result = await _controller.GetLicenseById(id);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedLicense = Assert.IsType<LicenseResponseDto>(okResult.Value);
            Assert.Equal(id, returnedLicense.Id);
        }

        [Fact]
        public async Task GetLicenseById_ReturnsNotFound_WhenLicenseDoesNotExist()
        {
            var id = Guid.NewGuid();
            _licenseServiceMock.Setup(s => s.GetByIdAsync(id)).ThrowsAsync(new KeyNotFoundException("Not found"));

            var result = await _controller.GetLicenseById(id);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Not found", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateLicense_ReturnsCreatedAtAction_WithCreatedLicense()
        {
            var licenseDto = new LicenseDto 
            { 
                GameId = Guid.NewGuid(), 
                Description = "New License", 
                Cost = 29.99m 
            };
            var createdLicense = new LicenseResponseDto 
            { 
                Id = Guid.NewGuid(), 
                GameId = licenseDto.GameId, 
                Description = licenseDto.Description, 
                Cost = licenseDto.Cost 
            };
            _licenseServiceMock.Setup(s => s.CreateAsync(licenseDto)).ReturnsAsync(createdLicense);

            var result = await _controller.CreateLicense(licenseDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<LicenseResponseDto>(createdAtActionResult.Value);
            Assert.Equal(createdLicense.Id, returnValue.Id);
            Assert.Equal("GetLicenseById", createdAtActionResult.ActionName);
            Assert.Equal(createdLicense.Id, createdAtActionResult.RouteValues["id"]);
        }

        [Theory]
        [InlineData(typeof(ValidationException), 400)]
        [InlineData(typeof(KeyNotFoundException), 404)]
        [InlineData(typeof(ArgumentException), 409)]
        public async Task CreateLicense_ReturnsExpectedStatusCode_OnException(Type exceptionType, int expectedStatusCode)
        {
            var licenseDto = new LicenseDto { Description = "Test", Cost = 10, GameId = Guid.NewGuid() };
            var exception = (Exception)Activator.CreateInstance(exceptionType, "Error message");
            _licenseServiceMock.Setup(s => s.CreateAsync(licenseDto)).ThrowsAsync(exception);

            var result = await _controller.CreateLicense(licenseDto);

            var objectResult = Assert.IsAssignableFrom<ObjectResult>(result.Result);
            Assert.Equal(expectedStatusCode, objectResult.StatusCode);
            Assert.Equal("Error message", objectResult.Value);
        }

        [Fact]
        public async Task UpdateLicense_ReturnsNoContent_OnSuccess()
        {
            var id = Guid.NewGuid();
            var licenseDto = new LicenseDto { Description = "Updated License", Cost = 50, GameId = Guid.NewGuid() };
            _licenseServiceMock.Setup(s => s.UpdateAsync(id, licenseDto)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateLicense(id, licenseDto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateLicense_ReturnsNotFound_WhenLicenseNotFound()
        {
            var id = Guid.NewGuid();
            var licenseDto = new LicenseDto { Description = "Updated License", Cost = 50, GameId = Guid.NewGuid() };
            _licenseServiceMock.Setup(s => s.UpdateAsync(id, licenseDto)).ThrowsAsync(new KeyNotFoundException("Not found"));

            var result = await _controller.UpdateLicense(id, licenseDto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Not found", notFoundResult.Value);
        }

        [Fact]
        public async Task PartialUpdateLicense_ReturnsBadRequest_WhenPatchDocIsNull()
        {
            var id = Guid.NewGuid();

            var result = await _controller.PartialUpdateLicense(id, null);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Patch document cannot be null", badRequestResult.Value);
        }

        [Fact]
        public async Task PartialUpdateLicense_ReturnsOk_WithUpdatedLicense()
        {
            var id = Guid.NewGuid();
            var patchDoc = new JsonPatchDocument<LicenseDto>();
            var updatedLicense = new LicenseResponseDto { Id = id, GameId = Guid.NewGuid(), Description = "Patched License", Cost = 55 };
            _licenseServiceMock.Setup(s => s.PatchAsync(id, patchDoc)).ReturnsAsync(updatedLicense);

            var result = await _controller.PartialUpdateLicense(id, patchDoc);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedLicense = Assert.IsType<LicenseResponseDto>(okResult.Value);
            Assert.Equal(id, returnedLicense.Id);
        }

        [Theory]
        [InlineData(typeof(KeyNotFoundException), 404)]
        [InlineData(typeof(ArgumentException), 409)]
        public async Task PartialUpdateLicense_ReturnsExpectedStatusCode_OnException(Type exceptionType, int expectedStatusCode)
        {
            var id = Guid.NewGuid();
            var patchDoc = new JsonPatchDocument<LicenseDto>();
            var exception = (Exception)Activator.CreateInstance(exceptionType, "Error message");
            _licenseServiceMock.Setup(s => s.PatchAsync(id, patchDoc)).ThrowsAsync(exception);

            var result = await _controller.PartialUpdateLicense(id, patchDoc);

            var objectResult = Assert.IsAssignableFrom<ObjectResult>(result);
            Assert.Equal(expectedStatusCode, objectResult.StatusCode);
            Assert.Equal(exception.Message, objectResult.Value);
        }

        [Fact]
        public async Task DeleteLicense_ReturnsNoContent_OnSuccess()
        {
            var id = Guid.NewGuid();
            _licenseServiceMock.Setup(s => s.DeleteAsync(id)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteLicense(id);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteLicense_ReturnsNotFound_WhenLicenseNotFound()
        {
            var id = Guid.NewGuid();
            _licenseServiceMock.Setup(s => s.DeleteAsync(id)).ThrowsAsync(new KeyNotFoundException("Not found"));

            var result = await _controller.DeleteLicense(id);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Not found", notFoundResult.Value);
        }
    }
}
