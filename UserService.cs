using Microsoft.Extensions.Primitives;

namespace OneHandTraining;

public interface IUserService
{
    Users UpdateUser(string token, UserRequestEnv<UserRequest> req);
    void Adduser(Users user);
    Users GetUser(string userToken);
    Users GetUserLogin(string email, string password);
    Users GetGeneric(Func<Users,bool> pred);
    void DeleteUser(string token);
}

public class UserService : IUserService
{
    private readonly IUsersRepository _usersRepository;

    public UserService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }
    public Users UpdateUser(string token, UserRequestEnv<UserRequest> req)
    {

        var user = _usersRepository.GetByToken(token);
        var resp = new Users(req.User.Username, req.User.Email, req.User.Password,$"{token}",user.Bio,user.Image);
        _usersRepository.Update(resp);
        return resp;
    }

    public void Adduser(Users user)
    {
        _usersRepository.Add(user);
    }

    public Users GetUser(string userToken)
    {
        return _usersRepository.GetByToken(userToken);
    }

    public Users GetUserLogin(string email, string password)
    {
        return _usersRepository.GetLoginUser(email, password);
    }

    public Users GetGeneric(Func<Users, bool> pred)
    {
        throw new NotImplementedException();
    }

    public void DeleteUser(string token)
    {
        _usersRepository.Delete(token);
    }
}