using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using server.Models;

namespace server.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Ringo Starr - Profile";
            ViewData["Bio"] = "English musician, singer, actor, songwriter, and drummer for the Beatles.";
            ViewData["Location"] = "Liverpool";
            ViewData["ProfPicURL"] = "https://placekitten.com/g/64/64";
            ViewData["Name"] = "Ringo Starr";
            ViewData["Posts"] = new List<object>();
            ViewData["Owner"] = "false";
            return View();
        }

        public IActionResult Venue()
        {
            ViewData["Title"] = "Ringo Starr - Profile";
            ViewData["Bio"] = "English musician, singer, actor, songwriter, and drummer for the Beatles.";
            ViewData["Location"] = "Liverpool";
            ViewData["ProfPicURL"] = "https://placekitten.com/g/64/64";
            ViewData["Name"] = "Ringo Starr";
            ViewData["Posts"] = new List<object>();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
