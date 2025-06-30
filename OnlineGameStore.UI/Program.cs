using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineGameStore.BLL;
using OnlineGameStore.BLL.Authorization;
using OnlineGameStore.DAL;
using OnlineGameStore.SharedLogic.Enums;
using OnlineGameStore.SharedLogic.Settings;
using OnlineGameStore.UI.Middleware;
using OnlineGameStore.UI.Services;

const string apiVersion = "1.0.0";
const string documentName = "online-game-store-api";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(documentName, new OpenApiInfo
    {
        Version = apiVersion,
        Title = "Online Game Store API",
        Description = "Swagger documentation for the Online Game Store API"
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "Enter 'Bearer' followed by a space and your JWT access token.\n\nExample: `Bearer eyJhbGciOiJIUzI1...`"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddDalServices(builder.Configuration);
builder.Services.AddBllServices();

builder.Services.AddSingleton<ExceptionHandlingMiddleware>();

builder.Services.AddHostedService<RoleSeederService>();
builder.Services.AddHostedService<AdminSeederService>();


//app.UseHttpsRedirection();

//app.UseCookiePolicy(new CookiePolicyOptions
//{
//    MinimumSameSitePolicy = SameSiteMode.Strict,
//    HttpOnly = HttpOnlyPolicy.Always,
//    Secure = CookieSecurePolicy.Always
//});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(JwtSettings.SecretKey))
        };
    });
builder.Services.AddAuthorization(options =>
{
    foreach (var permission in Enum.GetValues<PermissionEnum>())
    {
        options.AddPolicy($"Permissions.{permission}", policy =>
            policy.Requirements.Add(new PermissionRequirement(new[] { permission })));
    }
});

builder.Services.AddAuthorization();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint($"/swagger/{documentName}/swagger.json", $"OGS API v{apiVersion}");
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();
app.Run();

public partial class Program
{
}