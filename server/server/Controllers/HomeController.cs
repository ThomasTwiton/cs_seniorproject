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

        /*
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
    */




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId, Email, Password")] User user, string email, string password, string firstname, string lastname)
        { //IActionResult = promise. Create new instance of user class



            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync(); //wait until DB is saved for this result
                var user_with_profile = _context.Users.Include(p => p.Profile).Where(u => u.Email == email && u.Password == password).ToList();
                user_with_profile[0].Profile = new List<Profile>();
                Profile profile = new Profile();
                user_with_profile[0].Profile.Add(profile);
                profile.First_Name = firstname;
                profile.Last_Name = lastname;
                await _context.SaveChangesAsync(); //wait until DB is saved for this result
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Login(string email, string password)
        {


            var user_with_profile = _context.Users.Include(p => p.Profile).Where(u => u.Email == email && u.Password == password).ToList();
            var profile = user_with_profile[0].Profile.ToList()[0];
            var Ensembles = new List<String>();
            var dummyens = new Ensemble {Ensemble_Name = "Best Band Ever"};
            _context.Ensembles.Add(dummyens);
            if (profile.ProfileEnsemble == null)
            {
                profile.ProfileEnsemble = new List<ProfileEnsemble>
                {
                    new ProfileEnsemble{ProfileId = profile.ProfileId, EnsembleId = dummyens.EnsembleId}
                };
                profile.ProfileEnsemble.ToList().ForEach(pe => _context.ProfileEnsembles.Add(pe));
                await _context.SaveChangesAsync();
            }
            foreach (ProfileEnsemble pe in profile.ProfileEnsemble)
            {
                String ensemble = pe.Ensemble.Ensemble_Name;
                Ensembles.Add(ensemble);
            }


                
            if (profile == null)
            {
                return NotFound();
            }

            ViewData["first_name"] = profile.First_Name;
            ViewData["last_name"] = profile.Last_Name;
            ViewData["ensembles"] = Ensembles;

            return View();
        }

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
