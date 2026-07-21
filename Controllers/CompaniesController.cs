using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HipcallManagement.DTOs;
using HipcallManagement.Services;

namespace HipcallManagement.Controllers;

[Route("companies")]
public class CompaniesController : Controller
{
    private readonly IHipcallApiService _apiService;

    public CompaniesController(IHipcallApiService apiService)
    {
        _apiService = apiService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var companies = await _apiService.GetCompaniesAsync();
        return View(companies);
    }

    [HttpGet("new")]
    public IActionResult New()
    {
        return View(new CreateCompanyRequest());
    }

    [HttpPost("new")]
    public async Task<IActionResult> New(CreateCompanyRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        await _apiService.CreateCompanyAsync(request);
        return RedirectToAction(nameof(Index));
    }
}
