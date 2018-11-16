using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Login(string email, string password)
        {
            ProfileModel model = new ProfileModel();

            var user = _context.Users.Where(u => u.Email == email && u.Password == password).ToList()[0];
            model.User = user;

            //join the user and profile tables to get the profile
            var user_with_profile = _context.Users.Include(p => p.Profile).Where(u => u.Email == email && u.Password == password).ToList()[0];
            var profile = user_with_profile.Profile.ToList()[0];

            model.Profile = profile;

            var Ensembles = new List<Ensemble>();
            /*
            var dummyens = new Ensemble();
            dummyens.Ensemble_Name = "Best Band Ever";
            if (user_with_profile.Ensemble == null)
            {
                user_with_profile.Ensemble = new List<Ensemble>();

                user_with_profile.Ensemble.Add(dummyens);
                await _context.SaveChangesAsync();
            }

            if (profile.ProfileEnsemble == null)
            {
                profile.ProfileEnsemble = new List<ProfileEnsemble>();
                var dummymember = new ProfileEnsemble();
                dummymember.Profile = profile;
                dummymember.Ensemble = dummyens;
                profile.ProfileEnsemble.Add(dummymember);
                await _context.SaveChangesAsync();
            }
            */
            foreach (ProfileEnsemble pe in profile.ProfileEnsemble)
            {
                //Ensemble ensemble = pe.Ensemble.Ensemble
                //String ensembleN = pe.Ensemble.Ensemble_Name;
                //String ensembleID = pe.Ensemble.EnsembleId.ToString();
                //Console.WriteLine(ensemble);
                Ensembles.Add(pe.Ensemble);
            }
            model.Ensembles = Ensembles;
            



            if (profile == null)
            {
                return NotFound();
            }

            //ViewData["first_name"] = profile.First_Name;
            //ViewData["last_name"] = profile.Last_Name;


            /* The following are other bits of information
             * that are needed for the view. These are all
             * just templated code. */
            //ViewData["Title"] = "Ringo Starr - Profile";
            //ViewData["Bio"] = "English musician, singer, actor, songwriter, and drummer for the Beatles.";
            //ViewData["Location"] = "Liverpool";
            //ViewData["ProfPicURL"] = "https://placekitten.com/g/64/64";
            //ViewData["Owner"] = "true";
            //ViewData["ProfileType"] = "profile";

            return View(model);
        }


        public async Task<IActionResult> ViewEnsemble(int? id)
        {
            Console.WriteLine(id);
            if(id == null)
            {
                return NotFound();
            }

            var ensemble = await _context.Ensembles.FindAsync(id);
            Console.WriteLine(ensemble.Ensemble_Name);
            if (ensemble == null)
            {
                return NotFound();
            }
            return View(ensemble);
        }

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

    }
}
