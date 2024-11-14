using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MVCproyect.Models;
using MySql.Data.MySqlClient;
using MVCproyect.Services;

public class LoginController : Controller
{
    private readonly UserService _userService;
    private readonly MySqlService _mySqlService;

    public LoginController(UserService userService, MySqlService mySqlService)
    {
        _userService = userService;
        _mySqlService = mySqlService;
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

    public IActionResult DataBaseBackUp() 
    {
        _mySqlService.DataBaseBackUp();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult DataBaseRestore()
    {
        try
        {
            _mySqlService.RestoreDataBase();
        }
        catch (Exception ex) 
        {
            return RedirectToAction("Error","Home");
        }
        return RedirectToAction("Index", "Home");
    }
}
