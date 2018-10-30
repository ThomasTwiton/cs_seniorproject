using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using server.Models;


namespace server.Controllers
{
    public class HomeController : Controller
    {
        private readonly PluggedContext _context;

        public HomeController(PluggedContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Profile(string firstname, string lastname)
        { //find user with this info and get the rest of their info from DB
           /* using (var context = ){
                var user = context.Users
                                  .Where(u => u.Email == firstname && u.Password == lastname)
                                  .Include(u => u.Profile)
                                  .ToList();
                            
               
                foreach (User user1 in user)
                {
                    ViewData["Message"] = user1;
                }
    
            }
            */

            ViewData["Message"] = firstname + " " + lastname;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (string firstname, string lastname, string email, string password){ //IActionResult = promise. Create new instance of user class

            var user = new User();
            user.Email = email;
            user.Password = password;
            user.UserId = 99;

            var profile = new Profile();
            profile.UserId = 99;
            profile.First_Name = firstname;
            profile.Last_Name = lastname;

            Console.Write("NO");
            if (ModelState.IsValid)
            {
                _context.Add(user);
                _context.Add(profile);
                await _context.SaveChangesAsync(); //wait until DB is saved for this result
                return RedirectToAction(nameof(Index));
            }

            Console.Write("Hi");
            var usr_id = _context.Users.Where(u => u.Email == email && u.Password == password).Select(u => u.UserId).Single();



            return View(user);
        }

        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Email,Password")] User user, [Bind("UserId,Firstname, Lastname, ProfileId")] Profile profile)
        { //IActionResult = promise. Create new instance of user class

            if (ModelState.IsValid)
            {
                _context.Add(user);
                user.Profile.Add(profile);
                await _context.SaveChangesAsync(); //wait until DB is saved for this result
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        */

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }


        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
