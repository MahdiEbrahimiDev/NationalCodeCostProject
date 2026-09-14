using Microsoft.AspNetCore.Mvc;
using NationalCodeCostProject.Data;

namespace NationalCodeCostProject.Controllers;

public class UserController : Controller
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index() => View();

    [HttpPost]
    public IActionResult Index(string nationalCode)
    {
        var cost = _context.Costs
            .FirstOrDefault(x => x.NationalCode == nationalCode);

        if (cost != null)
            ViewBag.Amount = cost.Amount;
        else
            ViewBag.Message = "کد ملی یافت نشد";

        return View();
    }
}
