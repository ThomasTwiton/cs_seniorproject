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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email, Password")] User user)
        {
            /* This action method creates a new user in the database
             *  and moves the user to the profile creation view.
             */
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync(); //wait until DB is saved for this result
                /*
                var user_with_profile = _context.Users.Include(p => p.Profile).Where(u => u.Email == email && u.Password == password).ToList();
                user_with_profile[0].Profile = new List<Profile>();
                Profile profile = new Profile();
                user_with_profile[0].Profile.Add(profile);
                profile.First_Name = firstname;
                profile.Last_Name = lastname;
                await _context.SaveChangesAsync(); //wait until DB is saved for this result
                */
            }
            return RedirectToAction("CreateProfile", new { id = user.UserId });
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

        public IActionResult Profile(int? id)
        {
            Console.WriteLine("===================");
            Console.WriteLine(id);
            /* This action method displays the profile for the user with 
             *  the provided user id. Here the users should be able to 
             *  navagate to the following:
             *      - Profile   --> the profile img on the nav bar
             *      - Ensemble  --> any ensemble img in the ensembles area
             */
            ProfileModel model = new ProfileModel();
            //System.Web.HttpCookie myCookie = new HttpCookie("UserSettings");

            var user = _context.Users.Find(id);
            model.User = user;

            //join the user and profile tables to get the profile
            var user_with_profile = _context.Users.Include(p => p.Profile).Where(u => u.UserId == id).ToList()[0];
            var profile = user_with_profile.Profile.ToList()[0];
            //var profile = _context.Profiles.Find(id);

            model.Profile = profile;

            var Ensembles = new List<Ensemble>();

            if (profile.ProfileEnsemble == null)
            {
                profile.ProfileEnsemble = new HashSet<ProfileEnsemble>();
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
                Console.WriteLine(pe.Ensemble.Ensemble_Name);
                Ensembles.Add(pe.Ensemble);
            }
            model.Ensembles = Ensembles;

            if (profile == null)
            {
                return NotFound();
            }

            return View(model);
        }

        public async Task<IActionResult> ViewEnsemble(int? id)
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
            model.Instruments = _context.Instruments.ToList();
            if (id == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles.FindAsync(id);

            if (profile == null)
            {
                return NotFound();
            }

            if (profile.Plays_Instrument == null)
            {
                profile.Plays_Instrument = new List<Plays_Instrument>();
                foreach (Plays_Instrument pi in _context.Plays_Instruments)
                {
                    if(pi.ProfileId == profile.ProfileId)
                    {
                        pi.Instrument = _context.Instruments.Find(pi.InstrumentId);
                        profile.Plays_Instrument.Add(pi);
                    }
                }
                await _context.SaveChangesAsync();
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


        public IActionResult CreateProfile(int? id)
        {
            int? createdUserId = id;
            var lst = new List<string>();
            foreach(Instrument i in _context.Instruments)
            {
                lst.Add(i.Instrument_Name);
            }
            ViewData["Instruments"] = lst;
            ViewData["id"] = createdUserId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProfile(string pName, System.DateTime pBirthday, string pCity, string pState, string pBio, int userID)
        {
            Profile profile = new Profile();
            string[] nameList = pName.Split();
            profile.First_Name = nameList[0];
            profile.Last_Name = nameList[1];
            profile.City = pCity;
            profile.State = pState;
            profile.Bio = pBio;
            profile.User = _context.Users.Find(userID);

            _context.Add(profile);
            await _context.SaveChangesAsync();

            var lst = new List<string>();
            foreach (Instrument i in _context.Instruments)
            {
                lst.Add(i.Instrument_Name);
            }
            ViewData["Instruments"] = lst;
            return RedirectToAction("Profile", new { id = userID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEnsemble(string eName, System.DateTime eFormed, System.DateTime eDisbanded, string eCity, string eBio, string eState, string eType, string eGenre, int userID)
        {
            Ensemble ensemble = new Ensemble();
            ensemble.Ensemble_Name = eName;
            ensemble.Formed_Date = eFormed;
            ensemble.Disbanded_Date = eDisbanded;
            ensemble.Type = eType;
            ensemble.Genre = eGenre;
            ensemble.Bio = eBio;
            ensemble.City = eCity;
            ensemble.State = eState;
            ensemble.User = _context.Users.Find(userID);

          if (ModelState.IsValid)
            {
                _context.Add(ensemble);
                await _context.SaveChangesAsync();
            }

            var lst = new List<string>();
            foreach (Instrument i in _context.Instruments)
            {
                lst.Add(i.Instrument_Name);
            }
            ViewData["Instruments"] = lst;
            return View("CreateProfile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVenue(string vName, System.DateTime vFormed, string vAddr1, string vCity, string vState, string vPhone, string vWeb, string vBio, int userID)
        {
            Venue venue = new Venue();
            venue.Venue_Name = vName;
            venue.Address1 = vAddr1;
            venue.City = vCity;
            venue.State = vState;
            venue.Bio = vBio;
            venue.User = _context.Users.Find(userID);
            if (ModelState.IsValid)
            {
                _context.Add(venue);
                await _context.SaveChangesAsync();
            }

            var lst = new List<string>();
            foreach (Instrument i in _context.Instruments)
            {
                lst.Add(i.Instrument_Name);
            }
            ViewData["Instruments"] = lst;
            return View("CreateProfile");
        }

    }
}
