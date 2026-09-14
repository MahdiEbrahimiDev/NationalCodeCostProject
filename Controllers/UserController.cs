using System.ComponentModel.Design;
using Microsoft.AspNetCore.Mvc;
using NationalCodeCostProject.Data;

namespace NationalCodeCostProject.Controllers;

public class UserController : Controller
{
    private readonly IUserService _UserService;

    public UserController(IUserService UserService)
    {
        _UserService = UserService;
    }

    public IActionResult Index() => View();

    [HttpPost]
    
public async Task<IActionResult> Index(UserViewModel viewModel)
{
    if (!ModelState.IsValid)
        return View(viewModel);

    var res = await _UserService.GetPrice(viewModel);

    if (res != null)
        return View(res);

    ModelState.AddModelError("", "کد ملی پیدا نشد.");

    return View(viewModel);
}


}
