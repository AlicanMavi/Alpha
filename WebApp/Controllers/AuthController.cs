using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebApp.Controllers
{
    public class AuthController : Controller
    {
        
        public static List<(string Email, string Password)> Users = new List<(string Email, string Password)>
        {
            ("admin@example.com", "password123") 
        };

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserLoggedIn") == "true")
                return RedirectToAction("Projects", "Projects");

            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = Users.Find(u => u.Email == email && u.Password == password);

            if (user != default)
            {
                HttpContext.Session.SetString("UserLoggedIn", "true");
                HttpContext.Session.SetString("UserEmail", email); 
                return RedirectToAction("Projects", "Projects");
            }

            ViewBag.LoginError = "Invalid email or password.";
            return View();
        }


        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("UserLoggedIn") == "true")
                return RedirectToAction("Projects", "Projects");

            return View();
        }

        [HttpPost]
        public IActionResult Register(string fullname, string email, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ViewBag.RegisterError = "Passwords do not match.";
                return View();
            }

            if (Users.Exists(u => u.Email == email))
            {
                ViewBag.RegisterError = "Email is already registered.";
                return View();
            }

            Users.Add((email, password));
            TempData["RegisterSuccess"] = "Account created successfully! Please login.";
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }

    }
}