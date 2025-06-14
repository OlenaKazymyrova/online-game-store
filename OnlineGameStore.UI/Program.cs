using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using OnlineGameStore.BLL;
using OnlineGameStore.DAL;
using OnlineGameStore.UI.Services;
using OnlineGameStore.DAL.DBContext;

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
});


builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddDalServices(builder.Configuration);
builder.Services.AddBllServices();
builder.Services.AddHostedService<RoleSeederService>();

builder.Services.AddHostedService<AdminSeederService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint($"/swagger/{documentName}/swagger.json", $"OGS API v{apiVersion}");
    });
}
//app.UseHttpsRedirection();

app.MapControllers();

app.Run();

public partial class Program { }