using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.Configure<JsonSerializerOptions>(o => o.PropertyNameCaseInsensitive = true);
var app = builder.Build();


var UserDb = new List<Users>();

//automatically serialize from the body
app.MapPost("/users", async (HttpContext ctx,[FromBody] UserRequestEnv<UserRequest> req) =>
{
    var resp = new Users(req.User.Username, req.User.Email, req.User.Password,$"{Guid.NewGuid()}","","");
    UserDb.Add(resp);
    await ctx.Response.WriteAsJsonAsync(new UserRequestEnv<Users>(resp));
});

app.MapGet("/user",  (HttpRequest req) =>
{
    var user =UserDb.FirstOrDefault(x => x.Token == req.Headers["Authorization"]);
    return new UserRequestEnv<Users>(user);
});

//manually read/parse/deserialize direct from the body
app.MapPut("/user", async ( ctx) =>
{
    var body = "";
    using (var reader = new StreamReader(ctx.Request.Body))
    { 
        body = await reader.ReadToEndAsync();
    }
    
    var req = JsonSerializer.Deserialize<UserRequestEnv<UserRequest>>(body, new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
    var user =UserDb.FirstOrDefault(x => x.Token == ctx.Request.Headers["Authorization"]);
    UserDb.Remove(user);
    
    var resp = new Users(req.User.Username, req.User.Email, req.User.Password,$"{ctx.Request.Headers["Authorization"]}","","");
    UserDb.Add(resp);
    await ctx.Response.WriteAsJsonAsync( new UserRequestEnv<Users>(resp));
});

app.MapControllers();
app.Run("http://localhost:5500");


public class UserOld
{
    public readonly string Username;
    public  string Email { get; }
    public readonly string Password;

    public UserOld(string username,string email, string password)
    {
        Username = username;
        Email = email;
        Password = password;
    }
}

public record UserRequest (string Username, string Email, string Password);
public record UserRequestEnv<T> ( T User);

record Users(string? UserName, string? Email, string? Password, string? Token,
    string? Bio, string? Image);
    