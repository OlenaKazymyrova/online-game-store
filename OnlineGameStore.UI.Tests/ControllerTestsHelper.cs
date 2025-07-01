using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using OnlineGameStore.BLL.Authentication;
using OnlineGameStore.BLL.Authorization;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.SharedLogic.Enums;
using OnlineGameStore.SharedLogic.Interfaces;

namespace OnlineGameStore.UI.Tests;

public class ControllerTestsHelper<TService> : WebApplicationFactory<Program>
    where TService : class
{
    private readonly IMockCreator<TService> _mockCreator;

    public ControllerTestsHelper(IMockCreator<TService> mockCreator)
    {
        _mockCreator = mockCreator;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var serviceMock = _mockCreator.Create();

        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.AddDebug();
            logging.SetMinimumLevel(LogLevel.Debug);
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<TService>();
            services.AddSingleton(serviceMock);
            
            services.AddAuthentication("TestScheme")
                .AddScheme<AuthenticationSchemeOptions, AuthHandlerMock>("TestScheme", _ => { });
            
            services.RemoveAll<IPermissionService>();
            services.AddSingleton<IPermissionService>(new PermissionServiceMock());
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Permissions.Read", policy =>
                {
                    policy.Requirements.Add(new PermissionRequirement(new[] { PermissionEnum.Read }));
                    policy.AddAuthenticationSchemes("TestScheme");
                });
            });
            
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        });
    }
}