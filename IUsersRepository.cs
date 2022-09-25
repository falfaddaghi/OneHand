namespace OneHandTraining;

public interface IUsersRepository
{
    public int Add(Users entity);
    public void Delete(string entity);
    public void Update(Users entity);
    public Users GetByToken(string token);
    public Users GetLoginUser(string email, string password);
    public List<Users> GetUsersGeneric(Func<Users,bool> pred);  
}