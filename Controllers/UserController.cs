using Microsoft.AspNetCore.Mvc;
using MVCproyect.Interfaces;
using MVCproyect.Models;
using MVCproyect.Repository;

namespace MVCproyect.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly IUserRepository _userRepository;
        private List<User> _usersList = new();

        public UserController(UserService userService, UserRepository userRepository) 
        {
            _userService = userService;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            _usersList = await _userRepository.GetUsersAsync();
            ViewData["usersList"] = _usersList;
            return View();
        }

        public async Task<IActionResult> Create()
        {
            List<Role> roles = await _userService.GetUsersRolesAsync();

            ViewData["UserRoles"] = roles;

            return View("UserForm");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User newUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userRepository.AddUserAsync(newUser);
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "An error has occured: " + ex.Message.ToString() + "Product Id: " + newUser.UserId;
                    return View("ErrorView");
                }
            }

            return RedirectToAction("Index");
        }

        //public async Task<IActionResult> GetProductById(int id)
        //{
        //    Product product = await _productRepository.GetProductByIdAsync(id);
        //    return View("Product");
        //}
    }
}
