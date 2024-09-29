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
    public IActionResult Login(User user)
    {
        if (_userService.ValidateUser(user.UserName,user.PasswordHash))
        {
            user = _userService.GetUserByUsername(user.UserName);
            HttpContext.Session.SetString("Username", user.UserName);
            HttpContext.Session.SetInt32("Role", user.RoleId);
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
