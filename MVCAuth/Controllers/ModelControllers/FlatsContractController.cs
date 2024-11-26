using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCAuth.ModelViews;
using MVCAuth.Services;

namespace MVCAuth.Controllers.ModelControllers;

[ApiController]
[Route("/[controller]")]
public class FlatsContractController(FlatsContractService _service, 
    FlatService flatService, LandLordsAdditionalInfoService landLordsAdditionalService,
    LandLordService landLordService) : Controller
{

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        int pagenumber = Convert.ToInt32(HttpContext.Request.Query["pagenumber"]);
        List<FlatsContract> res = (List<FlatsContract>)await _service.GetFlatsContractFullInfosAsync(pagenumber, 20);
        List<LandLordsAdditionalInfo> landlords =  (List<LandLordsAdditionalInfo>) await landLordsAdditionalService.GetLandLordsAdditionalInfo();
        List<Flat> flats = (List<Flat>) await flatService.GetAllFlats();
        ContractModelView contracts = new ContractModelView
        {
            flatsContracts = res,
            LandLords = landlords,
            Flats = flats
        };
        return View(contracts);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> UpdateContract([FromBody] FlatsContract contract)
    {
        LandLord landlord = await landLordService.GetLandLord((int)contract.Llid);
        Flat flat = await flatService.GetFlat((int)contract.Fid);
        contract.Ll = landlord;
        contract.FidNavigation = flat;
        int res = await _service.UpdateFlatsContract(contract);

        Console.WriteLine("Contract updated");
        if (res == 0)
        {
            return NoContent();
        }

        return Ok();
    }

    
    [HttpDelete("/flatcontract/delete/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteContract(int id)
    {
        int res = await _service.DeleteFlatsContract(id);

        Console.WriteLine("Contract updated");
        if (res == 0)
        {
            return NoContent();
        }

        return Ok();
    }
}