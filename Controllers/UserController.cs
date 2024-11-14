﻿using Microsoft.AspNetCore.Mvc;
using MVCproyect.Interfaces;
using MVCproyect.Models;
using MVCproyect.Repository;
using MVCproyect.Services;
using MySqlX.XDevAPI;
using Newtonsoft.Json;

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
            int? loggedUserId = HttpContext.Session.GetInt32("UserId");
            List<Role> rolesList = new List<Role>();

            if (loggedUserId == null)
            {
                return RedirectToAction("Login", "Login");
            }

            User loggedUser = await _userRepository.GetUserByIdAsync(loggedUserId.Value);

            _usersList = await _userRepository.GetUsersAsync();

            rolesList = await _userService.GetUsersRolesAsync();

            ViewData["usersList"] = _usersList;

            ViewData["LoggedUser"] = loggedUser;

            ViewData["rolesList"] = rolesList; 

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

        public async Task<IActionResult> GetUserById(int id)
        {
            User user = await _userRepository.GetUserByIdAsync(id);
            return View("UserForm");
        }


        [HttpPost]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                User user = await _userRepository.GetUserByIdAsync(id);

                ViewBag.currentUser = user;

                ViewData["UserRoles"] = await _userService.GetUsersRolesAsync(); 
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("ErrorMessage");
            }

            return View("EditUserForm");
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUser(User updatedUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userRepository.UpdateUserAsync(updatedUser);
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "An error has occurred: " + ex.Message + " Sale Id: " + updatedUser.UserId;
                    return View("ErrorView");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userRepository.DeleteUserAsync(id);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("ErrorMessage");
            }

            return RedirectToAction("Index");
        }
    }
}
