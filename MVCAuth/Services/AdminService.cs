using Microsoft.EntityFrameworkCore;
using MVCAuth.Models.Users;
using MVCAuth.ModelViews;
using MVCAuth.Services.Jwt;

namespace MVCAuth.Services;

public class AdminService(UsersContext db, GiveToken giveToken)
{
    public async Task<string> GetToken(AdminModelView adminModelView)
    {
         var admin = db.Admins.FirstOrDefault(admin => admin.Login == adminModelView.Login && admin.Password == adminModelView.Password);
        if (admin == null)
        {
            return null;
        }
        
        string token = await giveToken.Token(adminModelView);
        
        return token;
    }
}