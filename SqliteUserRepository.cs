namespace OneHandTraining;

public class SqliteUserRepository: IUsersRepository
{
    private readonly OneHandContext _context;

    public SqliteUserRepository(OneHandContext context)
    {
        _context = context;
    }
    public int Add(Users entity)
    {

        _context.Users.Add(entity);
        _context.SaveChanges(); //unit of work (design pattern)
        return entity.UsersId; //yes magic
    }

    public void Delete(string entity)
    {
        var u = _context.Users.First(x=> x.Token == entity);
        _context.Users.Remove(u);
        _context.SaveChanges();
    }

    public void Delete(Users entity)
    {
        _context.Users.Remove(entity);
        _context.SaveChanges();
    }

    public void Update(Users entity)
    {
        _context.Users.Update(entity);
        _context.SaveChanges();
    }

    public Users GetByToken(string token)
    {
        return _context.Users.Single(x => x.Token == token);
    }

    public Users GetLoginUser(string email, string password)
    {
        return _context.Users.First(x => x.Email == email && x.Password == password);
    }

    public List<Users> GetUsersGeneric(Func<Users, bool> pred)
    {
        return _context.Users.Where(pred).ToList();
    }
}