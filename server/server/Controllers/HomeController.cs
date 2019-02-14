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

        public IActionResult Login(string email, string password)
        {
            /* This action method is used as a passageway from the landing
             *  page to a user's profile. It receives the user's credentials
             *  and logs them into the system. It returns the Profile view
             *  of the appropriate user.
             */
            ProfileModel model = new ProfileModel();

            var user = _context.Users.Where(u => u.Email == email && u.Password == password).ToList()[0];
            model.User = user;

            var profile = _context.Profiles.Where(p => p.UserId == user.UserId).ToList()[0];
            var proID = profile.ProfileId;

            return RedirectToAction("Profile", new { id = proID });
        }

        public IActionResult audsearch()
        {
            AuditionSearch model = new AuditionSearch();
            var auditions = _context.Auditions.ToList();
            foreach(Audition aud in auditions)
            {
                aud.Ensemble = _context.Ensembles.Find(aud.EnsembleId);
                aud.Instrument = _context.Instruments.Find(aud.InstrumentId);
            }
            model.Auditions = auditions;
            return View(model);
        }

        public IActionResult Profile(int? id)
        {
            /* This action method displays the profile for the user with 
             *  the provided user id. Here the users should be able to 
             *  navagate to the following:
             *      - Profile   --> the profile img on the nav bar
             *      - Ensemble  --> any ensemble img in the ensembles area
             */
            ProfileModel model = new ProfileModel();

            /* The following lines are the previous way in which we looked up a profile
             *   from the user id that was provided in the url. We have switched over to
             *   looking up a profile from the profile id provided. 
             *   
             * var user = _context.Users.Where(u => u.UserId == id).ToList()[0];
             * model.User = user;
             * //join the user and profile tables to get the profile
             * var user_with_profile = _context.Users.Include(p => p.Profile).Where(u => u.UserId == id).ToList()[0];
             * var profile = user_with_profile.Profile.ToList()[0];
             * 
             * We still need to make a way to get the viewer's information when looking at
             *   a page. Currently, we are hardcoding that information for demonstration 
             *   purposes as either UserId = 1 or isOwner = true;
             */

            var profile = _context.Profiles.Where(u => u.ProfileId == id).ToList()[0];

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
                Ensembles.Add(pe.Ensemble);
            }
            model.Ensembles = Ensembles;

            if (profile == null)
            {
                return NotFound();
            }

            List<Post> posts = new List<Post>();
            
            foreach(Post post in _context.Posts.OrderByDescending(p => p.PostId))
            {
                if (post.PosterType == "profile")
                {
                    int profileId = post.PosterIndex;
                    if (profileId == profile.ProfileId)
                    {
                        Profile poster_profile = _context.Profiles.Find(profileId);
                        post.Profile = poster_profile;
                        posts.Add(post);
                    }
                }
            }

            model.Posts = posts;

            model.ViewType = "profile";
            model.isOwner = true; //model.User.UserId == model.Profile.UserId;
            Console.WriteLine(model.isOwner);

            return View(model);
        }

        public IActionResult Ensemble(int? id)
        {
            /* This action method displays the profile for the ensemble with 
             *  the provided id. Here the users should be able to 
             *  navagate to the following:
             *      - Profile   --> the profile img on the nav bar *or* a profile in the members area
             *      - Audition  --> clicking on an audition posting
             */

            EnsembleModel model = new EnsembleModel();

            /* The following lines are the previous way in which we looked up an ensemble
             *   from the user id that was provided in the url. We have switched over to
             *   looking up an ensemble from the ensemble id provided. 
             * 
             * var user = _context.Users.Where(u => u.UserId == id).ToList()[0];
             * model.User = user;
             *
             *  //join the user and profile tables to get the profile
             *  var user_with_ensemble = _context.Users.Include(p => p.Ensemble).Where(u => u.UserId == id).ToList()[0];
             *  var ensemble = user_with_ensemble.Ensemble.ToList()[0];
             *  
             *  We still need to make a way to get the viewer's information when looking at
             *   a page. Currently, we are hardcoding that information for demonstration 
             *   purposes as either UserId = 1 or isOwner = true;
             *   
             */

            //TO DO ON MONDAY: populate audition data in EnsembleModel by querying _context for all auditions which have the given ensemble id
            var ensemble = _context.Ensembles.Where(u => u.EnsembleId == id).ToList()[0];

            model.Ensemble = ensemble;

            if(model.Ensemble.Audition == null)
            {
                model.Ensemble.Audition = new HashSet<Audition>();
                foreach(Audition ad in _context.Auditions)
                {
                    if (ad.EnsembleId == model.Ensemble.EnsembleId)
                    {
                        model.Ensemble.Audition.Add(ad);
                    }
                }
            }



            model.Instruments = new List<SelectListItem>();
            model.SelectedInsId = new List<String>();

            foreach (Instrument ins in _context.Instruments.ToList())
            {
                var ins_name = ins.Instrument_Name;
                SelectListItem chk_ins = new SelectListItem { Text = ins.Instrument_Name, Value = ins.InstrumentId.ToString() };
                model.Instruments.Add(chk_ins);
            }


            var Profiles = new List<Profile>();

            if (ensemble.ProfileEnsemble == null)
            {
                ensemble.ProfileEnsemble = new HashSet<ProfileEnsemble>();
                foreach (ProfileEnsemble pe in _context.ProfileEnsembles)
                {

                    if (pe.EnsembleId == ensemble.EnsembleId)
                    {
                        pe.Ensemble = _context.Ensembles.Find(pe.EnsembleId);
                        ensemble.ProfileEnsemble.Add(pe);

                    }
                }
            }

            Console.WriteLine("==============");
            foreach (ProfileEnsemble pe in ensemble.ProfileEnsemble)
            {
                if (pe.Profile == null)
                {
                    pe.Profile = _context.Profiles.Find(pe.ProfileId);
                }
                Profiles.Add(pe.Profile);
            }
            model.Profiles = Profiles;

            if (ensemble == null)
            {
                return NotFound();
            }

            HashSet<Post> posts = new HashSet<Post>();
            foreach (Post post in _context.Posts.OrderByDescending(p=>p.PostId))
            {
                if (post.PosterType == "ensemble")
                {
                    int ensembleId = post.PosterIndex;
                    if (ensembleId == ensemble.EnsembleId)
                    {
                        Ensemble poster_profile = _context.Ensembles.Find(ensembleId);
                        post.Ensemble = poster_profile;
                        posts.Add(post);

                        if (post.Type == "aud")
                        {
                            Audition aud = _context.Auditions.Find(post.Ref_Id);
                            post.Audition = aud;
                            posts.Add(post);
                        }
                    }

                }
            }
            Console.WriteLine("==================");
            Console.WriteLine(posts.Count);
            model.Posts = posts;

            model.ViewType = "ensemble";
            model.isOwner = true; //model.User.UserId == model.Ensemble.EnsembleId;

            return View(model);
        }


        public IActionResult Venue(int? id)
        {
            /* This action method displays the profile for the venue with 
             *  the provided id. Here the users should be able to 
             *  navagate to the following:
             *      - Profile   --> the profile img on the nav bar 
             *      - Gig  --> clicking on a gig posting
             */
            Console.WriteLine(id);
            VenueModel model = new VenueModel();

            var venue = _context.Venues.Where(u => u.VenueId == id).ToList()[0];

            List<Post> posts = new List<Post>();
            foreach (Post post in _context.Posts.OrderByDescending(p=>p.PostId))
            {
                if (post.PosterType == "venue")
                {
                    int venueId = post.PosterIndex;
                    if (venueId == venue.VenueId)
                    {
                        Venue poster_profile = _context.Venues.Find(venueId);
                        post.Venue = poster_profile;
                        posts.Add(post);
                    }
                }
            }
            Console.WriteLine("==================");
            Console.WriteLine(posts.Count);
            model.Posts = posts;

            model.Venue = venue;
            model.ViewType = "venue";
            model.IsOwner = true; //model.User.UserId == model.Venue.VenueId;

            return View(model);
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

                var lst = new List<string>();
                foreach (Instrument i in _context.Instruments)
                {
                    lst.Add(i.Instrument_Name);
                }

                ViewData["Instruments"] = lst;
                ViewData["id"] = user.UserId;
                return View("CreateProfile");
            }

            return View("index");
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

            return RedirectToAction("Profile", new { id = profile.ProfileId });
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

            return RedirectToAction("Ensemble", new { id = ensemble.EnsembleId });
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
            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();
            //if (ModelState.IsValid)
            //{
                
            //}

            return RedirectToAction("Venue", new { id = venue.VenueId });
        }

        public async Task<IActionResult> Audition(int id)
        {
            AuditionModel model = new AuditionModel();

            var aud = _context.Auditions.Where(u => u.AuditionId == id).ToList()[0];
            var ens = _context.Ensembles.Where(u => u.EnsembleId == aud.EnsembleId).ToList()[0];

            aud.Instrument = _context.Instruments.Find(aud.InstrumentId);

            var Profiles = new List<Profile>();

            if (ens.ProfileEnsemble == null)
            {
                ens.ProfileEnsemble = new HashSet<ProfileEnsemble>();
                foreach (ProfileEnsemble pe in _context.ProfileEnsembles)
                {

                    if (pe.EnsembleId == ens.EnsembleId)
                    {
                        pe.Ensemble = _context.Ensembles.Find(pe.EnsembleId);
                        ens.ProfileEnsemble.Add(pe);

                    }
                }
            }

            Console.WriteLine("==============");
            foreach (ProfileEnsemble pe in ens.ProfileEnsemble)
            {
                if (pe.Profile == null)
                {
                    pe.Profile = _context.Profiles.Find(pe.ProfileId);
                }
                Profiles.Add(pe.Profile);
            }
            model.Profiles = Profiles;
            model.Audition = aud;
            model.Ensemble = ens;
            model.ViewType = "ensemble";
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAudition(System.DateTime audition_date, System.DateTime closed_date, string location, string description, string eCity, int userID, int ensId, int selectedInsId)
        {


            Audition audition = new Audition();
            audition.Open_Date = audition_date;
            audition.Closed_Date = closed_date;
            audition.Audition_Location = location;
            audition.Audition_Description = description;
            audition.EnsembleId = ensId;
            audition.Instrument = _context.Instruments.Find(selectedInsId);

            if (ModelState.IsValid)
            {
                _context.Add(audition);
                await _context.SaveChangesAsync();
            }


            return RedirectToAction("Ensemble", new { id = ensId });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGig(System.DateTime audition_date, System.DateTime closed_date, string location, string eCity, string instrument, int userID)
        {
            return View("CreateProfile");
        }

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
                    if (pi.ProfileId == profile.ProfileId)
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
            model.Instruments = new List<SelectListItem>();
            model.SelectedInsIds = new List<String>();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProfileModel model)
        {
            /* This action method takes the updated info of the user's
             *  profile and saves it. There is no view associated with
             *  this method and the user should be redirected to their
             *  respective profile page.
             */

            var profile = _context.Profiles.Find(model.Profile.ProfileId);


            Profile userprofile = _context.Profiles.Find(model.Profile.ProfileId);
            userprofile.First_Name = model.Profile.First_Name;
            userprofile.Last_Name = model.Profile.Last_Name;
            userprofile.Preferred_Name = model.Profile.Preferred_Name;
            userprofile.Pic_Url = model.Profile.Pic_Url;
            await _context.SaveChangesAsync();

            foreach (Plays_Instrument pi in _context.Plays_Instruments)
            {
                if (pi.ProfileId == model.Profile.ProfileId)
                {
                    _context.Plays_Instruments.Remove(pi);

                }
            }
            await _context.SaveChangesAsync();

            userprofile.Plays_Instrument = new List<Plays_Instrument>();
            
            foreach (String ins in model.SelectedInsIds)
            {

                Plays_Instrument pi = new Plays_Instrument();
                pi.Profile = _context.Profiles.Find(model.Profile.ProfileId);
                pi.ProfileId = pi.Profile.ProfileId;
                pi.Instrument = _context.Instruments.Find(int.Parse(ins));
                pi.InstrumentId = int.Parse(ins);

                userprofile.Plays_Instrument.Add(pi);
            }
            await _context.SaveChangesAsync();
      
            return RedirectToAction("Edit", id = model.Profile.ProfileId);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> createPost([Bind("Text, PosterType, PosterIndex, Type")] Post post, string PosterType, int PosterIndex)
        {
            _context.Add(post);
            await _context.SaveChangesAsync();
            if(PosterType == "profile")
            {
                return RedirectToAction("Profile", new { id = PosterIndex });
            }
            if (PosterType == "ensemble")
            {
                return RedirectToAction("Ensemble", new { id = PosterIndex });
            }
            if (PosterType == "venue")
            {
                return RedirectToAction("Venue", new { id = PosterIndex });
            }
            //This is bad !!! X.X
            return View("Index");
        }
        
    }
}