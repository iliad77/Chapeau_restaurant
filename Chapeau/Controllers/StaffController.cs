using Chapeau.Models;
using Chapeau.Models.Enums;
using Chapeau.Repositories.Interface;
using Chapeau.Service.Interface;
using Chapeau.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Chapeau.Services.Interfaces;
using Chapeau.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau.Controllers
{
    public class StaffController : Controller
    {
        private IStaffService _staffService;
        private IRestaurantService _restaurantService;
        public StaffController(IStaffService staffService, IRestaurantService resService) 
        {
            _staffService = staffService;
            _restaurantService = resService;
        }
        public IActionResult Index()
        {
            try
            {
                List<User> staff = _staffService.GetAllStaff();
                return View(staff);
            }
            catch (Exception ex) { throw new Exception($"there was an error : {ex.Message}"); }

        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(User staff)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(staff.Username))
                {
                    ViewBag.ErrorMessage = "Username is required.";
                    return View();
                }

                if (string.IsNullOrWhiteSpace(staff.Password))
                {
                    ViewBag.ErrorMessage ="Password is required." ;
                    return View();
                }

                if (staff.Password.Length < 6)
                {
                    ViewBag.ErrorMessage = "Password must be at least 6 characters.";
                    return View(); 
                }

                _staffService.CreateStaff(staff);
                return RedirectToAction("Index");
            }
            catch (Exception ex) { throw new Exception($"there was an error : {ex.Message}"); }
        }
        

        [HttpGet]
        public IActionResult SignIn()
        {

            return View();
        }
        [HttpPost]
        public IActionResult SignIn(SignInViewModel credentials)
        {
            
        
            User? user = _staffService.GetbyCredentials(credentials.username, credentials.password);

            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid username or password.";
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role.ToString());

            TempData["logginConfirmation"] = "you've loggedd in successfully";

            return RedirectToAction("Index");
        
        }

        public IActionResult Logout() 
        {
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn", "Staff");
        }

        public IActionResult Dashboard(string message) 
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("SignIn");
            }
            return View();
        }

        public IActionResult RestaurantOverview(OrderStatus? status)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("SignIn");
            }

            List<RestaurantViewModel> orders = _restaurantService.GetRestaurantOverview(status);

            return View(orders);
        }


    }
}
