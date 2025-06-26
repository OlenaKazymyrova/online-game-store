using System.Net;
using System.Net.Http.Json;
using OnlineGameStore.BLL.DTOs.Users;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Tests.DataGenerators;
using OnlineGameStore.UI.Tests.ServiceMockCreators;

namespace OnlineGameStore.UI.Tests.Tests;

public class AuthControllerTests
{
    private readonly HttpClient _client;

    public AuthControllerTests()
    {
        var data = new UserEntityGenerator().Generate(100);
        var mockCreator = new UserServiceMockCreator(data);
        var factory = new ControllerTestsHelper<IUserService>(mockCreator);
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_CredentialsAreValid_ReturnsCreated()
    {
        var request = new UserCreateDto
        {
            Username = "qwerty_uiop",
            Email = "qwertyuiop@example.com",
            Password = "Password123"
        };

        var response = await _client.PostAsJsonAsync("api/auth/register", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdUser = await response.Content.ReadFromJsonAsync<UserReadDto>();

        Assert.NotNull(createdUser);
        Assert.Equal(request.Username, createdUser.Username);
        Assert.Equal(request.Email, createdUser.Email);
    }

    [Fact]
    public async Task Register_InvalidCredentials_ReturnsBadRequest()
    {
        UserCreateDto? request = null;

        var response = await _client.PostAsJsonAsync("api/auth/register", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Register_UsernameAlreadyExists_ReturnsBadRequest()
    {
        var request1 = new UserCreateDto
        {
            Username = "qwerty_uiop",
            Email = "qwertyuiop@example.com",
            Password = "Password123"
        };

        var request2 = new UserCreateDto
        {
            Username = "qwerty_uiop",
            Email = "qwertyuiopagain@example.com",
            Password = "Password123"
        };

        var response = await _client.PostAsJsonAsync("api/auth/register", request1);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var conflictResponse = await _client.PostAsJsonAsync("api/auth/register", request2);

        Assert.Equal(HttpStatusCode.Conflict, conflictResponse.StatusCode);
    }

    [Fact]
    public async Task Register_EmailAlreadyExists_ReturnsBadRequest()
    {
        var request1 = new UserCreateDto
        {
            Username = "qwerty_uiop",
            Email = "qwertyuiop@example.com",
            Password = "Password123"
        };

        var request2 = new UserCreateDto
        {
            Username = "qwerty_uiop_again",
            Email = "qwertyuiop@example.com",
            Password = "Password123"
        };

        var response = await _client.PostAsJsonAsync("api/auth/register", request1);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var conflictResponse = await _client.PostAsJsonAsync("api/auth/register", request2);

        Assert.Equal(HttpStatusCode.Conflict, conflictResponse.StatusCode);
    }
}