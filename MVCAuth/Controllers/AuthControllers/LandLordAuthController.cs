using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCAuth.ModelViews;
using MVCAuth.Services;
using MVCAuth.Services.Jwt;

namespace MVCAuth.Controllers.AuthControllers
{
    [ApiController]
    [Route("/auth/")]
    public class LandLordAuthController(LandLordService service) : Controller
    {
        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserModelView user)
        {
            if (user == null)
            {
                return BadRequest("There is no such user");
            }
            
            string token = await service.LoginLandlord(user);

            if (token == null)
            {
                return BadRequest("There is no such user");
            }
            
            HttpContext.Response.Cookies.Append(".AspNetCore.Application.Id", token, new CookieOptions
            {
                MaxAge = TimeSpan.FromMinutes(60)
            });
            return Redirect($"/Home/Index");
        }

        [HttpGet]
        [Route("register")]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            if (Request.Cookies[".AspNetCore.Application.Id"] != null)
            {
                Response.Cookies.Delete(".AspNetCore.Application.Id");
            }

            return Redirect("/Home/Index");
        }
    }
}
