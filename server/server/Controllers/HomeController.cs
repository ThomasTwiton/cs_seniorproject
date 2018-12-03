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
        // Initialize the context (aka database entity)
        private readonly PluggedContext _context;


        public HomeController(PluggedContext context)
        {
            // Load the context into the controller
            _context = context;
        }

        public IActionResult Index()
        {
            /* This action method is the landing page for our website. 
             * Here users should just be displayed the view as is and 
             *  can navigate to the following pages:
             *      - Login     --> By entering their login information.
             *      - Profile   --> By entering their information for a new account.
             */
            return View();
        }

        public IActionResult Privacy()
        {
            /* This action method is the page that displays our privacy
             *  policy. Currently, it has no view and is not accessible
             *  from links on our site, however, people can type the url
             *  in.
             */
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            /* This action method is used for debugging and displays any
             *  error information.
             */
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Login(string email, string password)
        {
            /* This action method is used as a passageway from the landing
             *  page to a user's profile. It receives the user's credentials
             *  and logs them into the system. It returns the Profile view
             *  of the appropriate user.
             */
            ProfileModel model = new ProfileModel();

            var user = _context.Users.Where(u => u.Email == email && u.Password == password).ToList()[0];
            model.User = user;

            return RedirectToAction("Profile", new { id = user.UserId });
        }

        public IActionResult Profile(int? id = 2)
        {
            /* This action method displays the profile for the user with 
             *  the provided user id. Here the users should be able to 
             *  navagate to the following:
             *      - Profile   --> the profile img on the nav bar
             *      - Ensemble  --> any ensemble img in the ensembles area
             */
            ProfileModel model = new ProfileModel();
            //System.Web.HttpCookie myCookie = new HttpCookie("UserSettings");

            var user = _context.Users.Where(u => u.UserId == id).ToList()[0];
            model.User = user;

            //join the user and profile tables to get the profile
            var user_with_profile = _context.Users.Include(p => p.Profile).Where(u => u.UserId == id).ToList()[0];
            var profile = user_with_profile.Profile.ToList()[0];

            model.Profile = profile;

            var Ensembles = new List<Ensemble>();


            if (profile.ProfileEnsemble == null)
            {
                profile.ProfileEnsemble = new List<ProfileEnsemble>();
                foreach (ProfileEnsemble pe in _context.ProfileEnsembles)
                {
                    if (pe.ProfileId == profile.ProfileId)
                    {
                        pe.Ensemble = _context.Ensembles.Find(pe.EnsembleId);
                        profile.ProfileEnsemble.Add(pe);
                    }
                }
            }

            foreach (ProfileEnsemble pe in profile.ProfileEnsemble)
            {
                Ensembles.Add(pe.Ensemble);
            }
            model.Ensembles = Ensembles;

            if (profile == null)
            {
                return NotFound();
            }

            return View(model);
        }

        public async Task<IActionResult> Ensemble(int? id)
        {
            /* This action method displays the profile for the ensemble with 
             *  the provided id. Here the users should be able to 
             *  navagate to the following:
             *      - Profile   --> the profile img on the nav bar *or* a profile in the members area
             *      - Audition  --> clicking on an audition posting
             */
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




        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            /* This action method displays view for editing the profile
             *  with the provided id. Here the users should be able to 
             *  navagate to the following:
             *      - Profile   --> nav bar
             *      - Edit[Post]--> submitting changes to the page
             */
            ProfileModel model = new ProfileModel();

            if (id == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles.FindAsync(id);

            if (profile == null)
            {
                return NotFound();
            }

            var play_ids = new List<int>();
            if (profile.Plays_Instrument == null)
            {
                profile.Plays_Instrument = new List<Plays_Instrument>();
                foreach (Plays_Instrument pi in _context.Plays_Instruments)
                {
                    if(pi.ProfileId == profile.ProfileId)
                    {
                        pi.Instrument = _context.Instruments.Find(pi.InstrumentId);
                        profile.Plays_Instrument.Add(pi);
                        play_ids.Add(pi.InstrumentId);
                    }
                }
                await _context.SaveChangesAsync();
            }

            else
            {
                foreach (Plays_Instrument pi in profile.Plays_Instrument)
                {
                    play_ids.Add(pi.InstrumentId);
                }
            }

            foreach (Instrument i in _context.Instruments.ToList())
            {
                var ins_name = i.Instrument_Name;
                SelectListItem chk_ins = new SelectListItem { Text = i.Instrument_Name, Value = i.InstrumentId.ToString() };
                if (play_ids.Contains(i.InstrumentId))
                {
                    chk_ins.Selected = true;
                }
                model.Instruments.Add(chk_ins);
            }


            model.Profile = profile;

            return View(model);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("First_Name,Last_Name,Preferred_Name, Plays_Instrument")] Profile profile)
        {
            /* This action method takes the updated info of the user's
             *  profile and saves it. There is no view associated with
             *  this method and the user should be redirected to their
             *  respective profile page.
             */
            Console.WriteLine(profile.First_Name);
            Console.WriteLine(profile.Plays_Instrument.ToList()[0].Instrument.Instrument_Name);
            /*
            if (id != movie.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            */
            return View("index");

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId, Email, Password")] User user, string email, string password, string firstname, string lastname)
        {
            /* This action method creates a new user in the database
             *  and moves the user to the profile creation view.
             */
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
