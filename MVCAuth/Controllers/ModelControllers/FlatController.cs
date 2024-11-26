using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCAuth.ModelViews;
using MVCAuth.Services;

namespace MVCAuth.Controllers.ModelControllers;

[ApiController]
[Route("/[controller]")]
public class FlatController(FlatService _service, LandLordsAdditionalInfoService _landLordService) : Controller
{

    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 252)]
    public async Task<IActionResult> Index()
    {
        int pagenumber = Convert.ToInt32(HttpContext.Request.Query["pagenumber"]);
        var flats = await _service.GetFlats(pagenumber, 20);
        if (flats != null)
        {
            return View(flats);
        }
        return View();
    }
    
    [HttpGet("/Flats/LandLord")]
    [Authorize(Roles = "User, Admin")]
    public async Task<IActionResult> LandLordIndex()
    {
        int pagenumber = 1;
        if(HttpContext != null)
        {
         pagenumber = Convert.ToInt32(HttpContext.Request.Query["pagenumber"]);

        }
        var flats = await _service.GetFlats(pagenumber, 20);
        var landlords = await _landLordService.GetLandLordsAdditionalInfo();
        FlatModelView flatModelView = new FlatModelView
        {
            Flats = flats,
            LandlordsInfo = landlords
        };

        if (flats != null)
        {
            return View(flatModelView);
        }
        return View();
    }

    [HttpPut]
    [Authorize(Roles = "User, Admin")]
    public async Task<ActionResult> Put([FromBody] Flat flat)
    {
        var res = await _service.UpdateFlat(flat);
        if (res == 0)
        {
            return NotFound();
        }
        return Ok(flat);
    }
    
    [HttpPost("/flat/add")]
    [Authorize(Roles = "User, Admin")]
    public async Task<ActionResult<Flat>> Add([FromBody] Flat flat)
    {
        Console.WriteLine(flat);
        await _service.AddFlat(flat);
 
        return Ok(flat);
    }

    [HttpGet("/flats/filter/{city?}/{averageCost:decimal?}")]
    public async Task<ActionResult<List<Flat>>> FlatFilter(string city, decimal averageCost)
    {
        List<Flat> res = await _service.GetByFilter(city, averageCost);
        ViewBag.city = city;
        if (res != null)
        {
            return View("Index", res);
        }
        return Redirect("/");
    }

    [HttpDelete("/flat/delete/{id:int}")]
    [Authorize(Roles = "User, Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        int res = await _service.DeleteFlat(id);

        if (res == 0)
        {
            return NotFound();
        }
        
        return Ok(res);
    }
}