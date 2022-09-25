namespace OneHandTraining;

public class SqlAuditDecorator: IUsersRepository 
{
    private readonly SqliteUserRepository _inner;

    public SqlAuditDecorator(SqliteUserRepository inner)
    {
        _inner = inner;
    }
    
    public int Add(Users entity)
    {
        if (entity.CreatedOn == null || entity.CreatedOn == default(DateTime))
        {
            entity.CreatedOn = DateTime.Now;
        }
        entity.ModifiedOn = DateTime.Now;
        var x = _inner.Add(entity);
        return x;
    }

    public void Delete(string entity)
    {
        _inner.Delete(entity);
    }

    public void Update(Users entity)
    {
        _inner.Update(entity);
    }

    public Users GetByToken(string token)
    {
        return _inner.GetByToken(token);
    }

    public Users GetLoginUser(string email, string password)
    {
        return _inner.GetLoginUser(email, password);
    }


    public List<Users> GetUsersGeneric(Func<Users, bool> pred)
    {
        return _inner.GetUsersGeneric(pred);
    }
}
