using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCAuth.Services;

namespace MVCAuth.Controllers;

[ApiController]
[Route("/[controller]")]
public class HouseController(HouseService _service) : Controller
{

    public async Task<IActionResult> Index()
    {
        int pagenumber = Convert.ToInt32(HttpContext.Request.Query["pagenumber"]);
        var houses = await _service.GetHouses(pagenumber, 20);
        if (houses != null)
        {
            return View(houses);
        }
        return View();
    }

    [HttpGet("/Houses/LandLord")]
    [Authorize(Roles = "User, Admin")]
    public async Task<IActionResult> LandLordIndex()
    {
        int pagenumber = Convert.ToInt32(HttpContext.Request.Query["pagenumber"]);
        var houses = await _service.GetHouses(pagenumber, 20);
        if (houses != null)
        {
            return View(houses);
        }
        return View();
    }
    
    [HttpPut]
    [Authorize(Roles = "User, Admin")]
    public async Task<ActionResult> Put([FromBody] House house)
    {
        var res = await _service.UpdateHouse(house);
        if (res == 0)
        {
            return NotFound();
        }
        return Ok(house);
    }
    
    [HttpGet("/houses/filter/{city?}/{averageCost:decimal?}")]
    public async Task<ActionResult<List<House>>> HouseFilter(string city, decimal averageCost)
    {
        List<House> res = await _service.GetByFilter(city, averageCost);
        ViewBag.city = city;

        if (res != null)
        {
            return View(res);
        }
        return View();
    }

    [HttpDelete("/house/delete/{id:int}")]
    [Authorize(Roles = "User, Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        int res = await _service.DeleteHouse(id);

        if (res == 0)
        {
            return NotFound();
        }
        return Ok(res);
    }
}