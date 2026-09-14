using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace NationalCodeCostProject.Controllers;

public class AccountController : Controller
{
      private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }
    public IActionResult Register() => View();
    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var success = await _userService.Register(dto);
        if (!success)
        {
            ViewBag.Message = "ثبت‌نام ناموفق بود (یوزرنیم تکراری یا پسوردها مطابقت ندارند)";
            return View();
        }

        return RedirectToAction("Login");
    }
    public IActionResult Login() => View();
    [HttpPost]
    public IActionResult Login(registerDto dto)
    {
        var user = await _userService.Login(dto);
        if (user == null)
        {
            ViewBag.Message = "نام کاربری یا رمز عبور اشتباه است";
            return View();
        }

       var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };
       var identity= new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
       var principal=new ClaimsPrincipal(identity);
       await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
       return RedirectToAction("Upload");
    }

    public IActionResult Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Login");
    }
}
