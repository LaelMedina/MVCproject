using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using MVCproyect.Interfaces;
using MVCproyect.Models;
using MVCproyect.Repository;

namespace MVCproyect.Controllers
{
    public class SellerController : Controller
    {
        private readonly SellerRepository _sellerRepository;
        private readonly UserRepository _userRepository;

        public SellerController(SellerRepository sellerRepository, UserRepository userRepository) 
        { 
            _sellerRepository = sellerRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            List<Seller> sellers = new();
            try
            {
                int? loggedUserId = HttpContext.Session.GetInt32("UserId");
                List<Role> rolesList = new List<Role>();

                if (loggedUserId == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                User loggedUser = await _userRepository.GetUserByIdAsync(loggedUserId.Value);

                sellers = await _sellerRepository.GetSellersAsync();

                ViewData["LoggedUser"] = loggedUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            ViewData["Sellers"] = sellers;
            return View();
        }

        public IActionResult Create() 
        {
            return View("SellerForm");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller newSeller)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _sellerRepository.AddSellerAsync(newSeller);
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "An error has occured: " + ex.Message.ToString() + "Product Id: " + newSeller.Id;
                    return View("ErrorView");
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id) 
        {
            try
            {
                Seller seller = await _sellerRepository.GetSellerByIdAsync(id);

                ViewBag.currentSeller = seller;

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("ErrorMessage");
            }

            return View("EditSellerForm");
        }

        [HttpPost]
        public async Task<ActionResult> UpdateSeller(Seller updatedSeller)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _sellerRepository.UpdateSellerAsync(updatedSeller);
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "An error has occurred: " + ex.Message + " Sale Id: " + updatedSeller.Id;
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
                await _sellerRepository.DeleteSellerAsync(id);
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
