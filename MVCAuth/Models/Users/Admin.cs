using System.ComponentModel.DataAnnotations;

namespace MVCAuth.Models.Users;

public class Admin
{
    [Key]
    public int Id { get; set; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string? Role { get; private set; }
    
    public Admin()
    {
        Role = "Admin";
    }
}