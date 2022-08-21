using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace OneHandTraining;

[ApiController]
public class UsersController : ControllerBase
{
    private List<Users> UserDb = new List<Users>();
    
    [HttpGet]
    [Route("/MVC/users")]
    public ActionResult Foo()
    {
        var user =UserDb.FirstOrDefault(x => x.Token == this.Request.Headers["Authorization"]);
        return new JsonResult(new UserRequestEnv<Users>(user));
    }

    [HttpPost]
    [Route("/MVC/user")]
    public ActionResult Post([FromBody] UserRequestEnv<UserRequest> req)
    {
        var resp = new Users(req.User.Username, req.User.Email, req.User.Password,$"{Guid.NewGuid()}","","");
        UserDb.Add(resp);
        return new JsonResult(new UserRequestEnv<Users>(resp));
    }

    [HttpPut]
    [Route("/MVC/user")]
    public async Task<ActionResult> Put()
    {
        var body = "";
        using (var reader = new StreamReader(this.Request.Body))
        { 
            body = await reader.ReadToEndAsync();
        }

        var req = JsonSerializer.Deserialize<UserRequestEnv<UserRequest>>(body, new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
        var user =UserDb.FirstOrDefault(x => x.Token == this.Request.Headers["Authorization"]);
        UserDb.Remove(user);
    
        var resp = new Users(req.User.Username, req.User.Email, req.User.Password,$"{this.Request.Headers["Authorization"]}","","");
        UserDb.Add(resp);
        return  new JsonResult(new UserRequestEnv<Users>(resp));
    }
    
}