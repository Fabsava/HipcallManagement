using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HipcallManagement.DTOs;
using HipcallManagement.Services;

namespace HipcallManagement.Controllers;

public class CompaniesController : Controller
{
    private readonly IHipcallApiService _apiService;

    public CompaniesController(IHipcallApiService apiService)
    {
        _apiService = apiService;
    }

    [HttpGet]
    [Route("")]
    [Route("companies")]
    public async Task<IActionResult> Index()
    {
        var companies = await _apiService.GetCompaniesAsync();
        return View(companies);
    }

    [HttpGet]
    [Route("companies/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var company = await _apiService.GetCompanyByIdAsync(id);
        if (company == null)
        {
            return NotFound();
        }
        return View(company);
    }

    [HttpGet]
    [Route("companies/new")]
    public async Task<IActionResult> New()
    {
        ViewBag.Users = await _apiService.GetUsersAsync();
        return View(new CreateCompanyRequest());
    }

    [HttpPost]
    [Route("companies/new")]
    public async Task<IActionResult> New(CreateCompanyRequest request)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Users = await _apiService.GetUsersAsync();
            return View(request);
        }

        if (request.Emails != null)
        {
            request.Emails.RemoveAll(e => string.IsNullOrWhiteSpace(e.Email));
        }

        if (request.Phones != null)
        {
            request.Phones.RemoveAll(p => string.IsNullOrWhiteSpace(p.Number) && string.IsNullOrWhiteSpace(p.Country));

            foreach (var phone in request.Phones)
            {
                if (phone.Country == "90" || string.IsNullOrWhiteSpace(phone.Country))
                {
                    phone.Country = "TR";
                }

                if (!string.IsNullOrWhiteSpace(phone.Number))
                {
                    var cleanNum = phone.Number.Trim();
                    if (!cleanNum.StartsWith("+"))
                    {
                        if (cleanNum.StartsWith("0"))
                        {
                            cleanNum = cleanNum.Substring(1);
                        }

                        if (phone.Country.Equals("TR", System.StringComparison.OrdinalIgnoreCase) && !cleanNum.StartsWith("90"))
                        {
                            cleanNum = "+90" + cleanNum;
                        }
                        else
                        {
                            cleanNum = "+" + cleanNum;
                        }
                    }
                    phone.Number = cleanNum;
                }
            }
        }

        try
        {
            await _apiService.CreateCompanyAsync(request);
            return Redirect("/companies");
        }
        catch (System.Exception ex)
        {
            ViewBag.Users = await _apiService.GetUsersAsync();
            ModelState.AddModelError(string.Empty, "Kaydetme başarısız: " + ex.Message);
            return View(request);
        }
    }
}
