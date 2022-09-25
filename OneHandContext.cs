using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace OneHandTraining;
//https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli
public class OneHandContext: DbContext
{
    public DbSet<Users> Users { get; set; }

    public string DbPath { get; set; }
    
    public OneHandContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "blogging.db");
    }
    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    
}

public class Auditable
{
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
}

public class Users : Auditable
{
    public Users(string userName, string email, string password, string token, string bio, string image)
    {
        UserName = userName;
        Email = email;
        Password = password;
        Token = token;
        Bio = bio;
        Image = image;
    }

    public int UsersId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Token { get; set; }
    public string Bio { get; set; }
    public string Image { get; set; }
    [NotMapped]
    public List<string> Following { get; set; } = new();
    
}