using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var a = User.Claims;
            var claimedHobby = User.Claims.FirstOrDefault(c => c.Type == "Hobby");
            var isFishing = claimedHobby?.Value == "Fishing";
            return View();
        }

        public IActionResult Login()
        {
            //User Validated successfuly
            //Data fetched from DB

            var jobClaims= new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Krzysiek"),
                new Claim("Job", "Mentor"),
                new Claim(ClaimTypes.Email, "kjaroska@gmail.com"),
                new Claim(ClaimTypes.Role,  "Boss")

            };

            var personalClaims = new List<Claim>()
            {
                new Claim("Hobby", "Fish2ing"),
                new Claim("DriverLicense", "B+E")
            };

            var myIdentity = new ClaimsIdentity(jobClaims, "My Claims Identity");
            var myIdentity2 = new ClaimsIdentity(personalClaims, "My Claims Identity");

            var principal = new ClaimsPrincipal(new [] {myIdentity, myIdentity2});
            HttpContext.SignInAsync(principal);

            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            var a = User.Identity;
            HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Policy = "FishermanOnly")]
        public IActionResult Secure()
        {
            return View();
        }
    }
}
