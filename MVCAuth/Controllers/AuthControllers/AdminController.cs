using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCAuth.ModelViews;
using MVCAuth.Services;

namespace MVCAuth.Controllers.AuthControllers;

[ApiController]
[Route("/[controller]/")]
[Authorize(Roles = "Admin")]
public class AdminController(LandLordService service, AdminService adminService) : Controller
{
    [Route("Login")]
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }
    
    [Route("Login")]
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] AdminModelView adminModelView)
    {
        if (adminModelView == null)
        {
            return BadRequest();
        }
        string token = await adminService.GetToken(adminModelView);
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
    
    [Route("/landLords")]
    public async Task<IActionResult> LandLords()
    {
        int pagenumber = Convert.ToInt32(HttpContext.Request.Query["pagenumber"]);
        var landLordsWithInfo = await service.GetLandLordsWithInfo(pagenumber, 20);
        return View(landLordsWithInfo);
    }

    [Route("logout")]
    public IActionResult Logout()
    {
        if (Request.Cookies.ContainsKey(".AspNetCore.Application.Id"))
        {
            HttpContext.Response.Cookies.Delete(".AspNetCore.Application.Id");
        }
        
        return Redirect("/Home/Index");
    }

    [Route("update/landlord")]
    [HttpPut]
    public async Task<IActionResult> AdminUpdateLandLord([FromBody] LandLordDto landlordDto)
    {
        LandLord landlord = await service.GetLandLord(landlordDto.Id);
        if (landlord == null)
        {
            return BadRequest();
        }
        landlord.Login = landlordDto.Login;
        landlord.Password = landlordDto.Password;
        Console.WriteLine(landlordDto.Password);
        landlord.Email = landlordDto.Email;
        int resp = await service.UpdateLandLord(landlord);
        
        return Ok(resp);
    }
}