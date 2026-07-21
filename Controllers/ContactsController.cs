using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HipcallManagement.DTOs;
using HipcallManagement.Services;

namespace HipcallManagement.Controllers;

public class ContactsController : Controller
{
    private readonly IHipcallApiService _apiService;

    public ContactsController(IHipcallApiService apiService)
    {
        _apiService = apiService;
    }

    [HttpGet]
    [Route("contacts")]
    public async Task<IActionResult> Index()
    {
        var contacts = await _apiService.GetContactsAsync();
        return View(contacts);
    }

    [HttpGet]
    [Route("contacts/new")]
    public IActionResult New()
    {
        return View(new CreateContactRequest());
    }

    [HttpPost]
    [Route("contacts/new")]
    public async Task<IActionResult> New(CreateContactRequest request)
    {
        if (!ModelState.IsValid)
        {
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
            await _apiService.CreateContactAsync(request);
            return Redirect("/contacts");
        }
        catch (System.Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Kaydetme başarısız: " + ex.Message);
            return View(request);
        }
    }
}
