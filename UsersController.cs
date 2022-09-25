using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace OneHandTraining;

[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly OneHandContext _context;

    public UsersController(IUserService userService, OneHandContext context)
    {
        _userService = userService;
        _context = context;
    }

    [HttpGet]
    [Route("/user")]
    public ActionResult Get()
    {
        var user = _userService.GetUser(this.Request.Headers["Authorization"]); 
        //var user2 = _userService.GetGeneric(XmlConfigurationExtensions=> XmlConfigurationExtensions.Token ==this.Request.Headers["Authorization"]); 
        return new JsonResult(new UserRequestEnv<Users>(user));
    }

    [HttpPost]
    [Route("/users")]
    public ActionResult Post([FromBody] UserRequestEnv<UserRequest> req)
    {
        var resp = new Users(req.User.Username, req.User.Email, req.User.Password,$"{Guid.NewGuid()}","","");
        resp.CreatedOn = DateTime.Now;
        _context.Users.Add(resp);
        _context.SaveChanges();
        _userService.Adduser(resp);
        
        var response = UserDtoMapper(resp);
        
        return new JsonResult(new UserRequestEnv<UserResponse>(response));
    }
    
    private UserResponse UserDtoMapper(Users resp)
    {

        return new UserResponse(resp.UserName, resp.Email, resp.Token, resp.Bio, resp.Image);
    } 

    [HttpPut]
    [Route("/user")]
    public async Task<ActionResult> Put()
    {
        var body = "";
        using (var reader = new StreamReader(this.Request.Body))
        { 
            body = await reader.ReadToEndAsync();
        }

        var req = JsonSerializer.Deserialize<UserRequestEnv<UserRequest>>(body, new JsonSerializerOptions{PropertyNameCaseInsensitive = true});

        var resp = _userService.UpdateUser(this.Request.Headers["Authorization"], req);
        
        return  new JsonResult(new UserRequestEnv<Users>(resp));
    }
    [HttpPost]
    [Route("/users/login")]
    public async Task<ActionResult> Login(UserRequestEnv<LoginRequest> req)
    {
        var user = _userService.GetUserLogin(req.User.Email, req.User.Password); 
        //var user2 = _userService.GetGeneric(x=> x.Email== req.User.Email && x.Password==req.User.Password); 
        if (user is null)
        {
            return new ForbidResult();
        }
        return  new JsonResult(new UserRequestEnv<Users>(user));
    }
}

public record LoginRequest(string Email, string Password);

public record UserRequest (string Username, string Email, string Password);

public record UserRequestEnv<T> ( T User);
public record ProfileRequestEnv<T> ( T Profile);

// public record Users(string? UserName, string? Email, string? Password, string? Token,
//     string? Bio, string? Image, List<string> Following= null);

public record UserResponse(string? UserName, string? Email, string? Token,
    string? Bio, string? Image);
