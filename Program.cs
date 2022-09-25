using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using OneHandTraining;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.Configure<JsonSerializerOptions>(o => o.PropertyNameCaseInsensitive = true);
builder.Services.AddDbContext<OneHandContext>();
builder.Services.AddScoped<IUsersRepository, SqliteUserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.MapControllers();
app.Run("http://localhost:5500");