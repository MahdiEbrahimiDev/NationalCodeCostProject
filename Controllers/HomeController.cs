using Microsoft.AspNetCore.Mvc;

namespace NationalCodeCostProject.Controllers;

public class HomeController : Controller
{
    public IActionResult Error()
    {
        return View();
    }
}