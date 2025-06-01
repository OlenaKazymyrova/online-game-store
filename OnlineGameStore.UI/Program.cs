using OnlineGameStore.BLL;
using OnlineGameStore.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDalServices(builder.Configuration);
builder.Services.AddBllServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

public partial class Program { }