namespace ProjectIOU;

using System.IO;

public class User
{
    public Guid ID {get; set;}
    public string Username {get; set;} = string.Empty;
    public string Password {get; set;} = string.Empty;

    public User()
    {
        ID = Guid.NewGuid();
    } 
}