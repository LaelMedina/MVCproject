using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MVCproyect.Models;

public class LoginController : Controller
{
    private readonly UserService _userService;

    public LoginController(UserService userService)
    {
        _userService = userService;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(User user)
    {
        if(await _userService.ValidateUserAsync(user.UserName,user.PasswordHash))
        {
            user = await _userService.GetUserByNameAsync(user.UserName);
            HttpContext.Session.SetString("Username", user.UserName);
            HttpContext.Session.SetInt32("UserRoleId", user.RoleId);
            HttpContext.Session.SetInt32("UserId", user.UserId);
            return RedirectToAction("Index", "Home");
        }
        ViewBag.ErrorMessage = "Invalid credentials";
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
