using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using server.Models;
using System.Text;

namespace server.Controllers
{

    public class HomeController : Controller
    {
        // Initialize the context (aka database entity)
        private readonly PluggedContext _context;
        private IHostingEnvironment _hostingEnvironment;

        private const string CookieUserId = "_UserID";
        private const string CookiePrevAct = "_PrevAction";

        public SessionModel GetSessionInfo(HttpRequest s)
        {
            SessionModel ret = new SessionModel();

            // Get the encrypted values
            string uidString = s.Cookies[CookieUserId];

            // Decrypt PrevAction
            ret.PrevAction = s.Cookies[CookiePrevAct];

            if (uidString != null & uidString != "")
            {

                ret.IsLoggedIn = true;
                ret.UserID = ret.UserID = Int32.Parse(uidString);

            } else
            {
                // No UserID means the user is not logged in
                ret.IsLoggedIn = false;
            }

            return ret;

        }

        private static string CreateSalt(int size)
        {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
            return Convert.ToBase64String(buff);
        }

        private static string CreatePasswordHash(string pwd, string salt)
        {
            string saltAndPwd = pwd + salt;
            byte[] bytes = Encoding.ASCII.GetBytes(saltAndPwd);
            SHA512 shaM = new SHA512Managed();
            byte[] hashedPwdbytes = shaM.ComputeHash(bytes);
            string hashedPwd = Convert.ToBase64String(hashedPwdbytes);
            return hashedPwd;
        }

        public HomeController(PluggedContext context, IHostingEnvironment environment)
        {
            // Load the context into the controller
            _context = context;

            //Load the enviornment into the controller
            _hostingEnvironment = environment;
        }

        public IActionResult Index()
        {
            /* This action method is the landing page for our website. 
             * Here users should just be displayed the view as is and 
             *  can navigate to the following pages:
             *      - Login     --> By entering their login information.
             *      - Profile   --> By entering their information for a new account.
             */

            SessionModel s = GetSessionInfo(Request);

            if (s.IsLoggedIn)
            {
                // Check if there are any profiles associated with the user id
                if (_context.Profiles.Any(p => p.UserId == s.UserID))
                {
                    var profile = _context.Profiles.Where(p => p.UserId == s.UserID).ToList()[0];
                    return RedirectToAction("Profile", new { id = profile.ProfileId });
                }

                // Check if there are any venues associated with the user id
                if (_context.Venues.Any(p => p.UserId == s.UserID))
                {
                    var venue = _context.Venues.Where(p => p.UserId == s.UserID).ToList()[0];
                    return RedirectToAction("Venue", new { id = venue.VenueId });
                }

                // Check if there are any ensembles associated with the user id
                if (_context.Ensembles.Any(p => p.UserId == s.UserID))
                {
                    var ensemble = _context.Ensembles.Where(p => p.UserId == s.UserID).ToList()[0];
                    return RedirectToAction("Ensemble", new { id = ensemble.EnsembleId });
                }
            }

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
        public IActionResult Login(string email, string password)
        {
            /* This action method is used as a passageway from the landing
             *  page to a user's profile. It receives the user's credentials
             *  and logs them into the system. The login information (id, 
             *  viewType and viewID) is stored in the Session in key/value
             *  pairs. All keys are stored as strings and all values are 
             *  stored as bytes.
             *  
             *  id:         The user's ID
             *  viewType:   The type of view that the user is logged in as
             *              (ie "ensemble", "profile", "venue")
             *  viewID:     The ID that is associated with the appropriate viewType
             *  
             *  This action returns the Profile view of the appropriate user.
             */
            
            if (ModelState.IsValid)
            {
                ProfileModel model = new ProfileModel();

                List<User> users = _context.Users.Where(u => u.Email == email).ToList();
                if(users.Count() == 0)
                {
                    ViewData["Error"] = "Email not registered";
                    return View("Index");
                }
                User user = users[0];

                string salt = user.Salt;

                if (user.Password != CreatePasswordHash(password, salt)){
                    ViewData["Error"] = "Username and password do not match";
                    return View("Index");
                }
                
                model.User = user;

                string encUID = user.UserId.ToString();

                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(1);
                option.IsEssential = true;
                Response.Cookies.Append(CookieUserId, encUID, option);
            
                SessionModel s = GetSessionInfo(Request);
                Console.WriteLine(s.PrevAction);

                if (s.PrevAction != null && s.PrevAction != "")
                {
                    // If the user tried to do something without being logged in,
                    //  then they should be redirected back to that thing.
                    
                    return Redirect(s.PrevAction);
                }

                // Check if there are any profiles associated with the user id
                if (_context.Profiles.Any(p => p.UserId == user.UserId))
                {
                    var profile = _context.Profiles.Where(p => p.UserId == user.UserId).ToList()[0];
                    return RedirectToAction("Profile", new { id = profile.ProfileId });
                }

                // Check if there are any venues associated with the user id
                else if (_context.Venues.Any(p => p.UserId == user.UserId))
                {
                    var venue = _context.Venues.Where(p => p.UserId == user.UserId).ToList()[0];
                    return RedirectToAction("Venue", new { id = venue.VenueId });
                }

                // Check if there are any ensembles associated with the user id
                else if (_context.Ensembles.Any(p => p.UserId == user.UserId))
                {
                    var ensemble = _context.Ensembles.Where(p => p.UserId == user.UserId).ToList()[0];
                    return RedirectToAction("Ensemble", new { id = ensemble.EnsembleId });
                }

                // If there are no entities associated with this user id,
                //  then send them to the 'CreateProfile' page to make an entity
                else
                {
                    var lst = new List<string>();
                    foreach (Instrument i in _context.Instruments)
                    {
                        lst.Add(i.Instrument_Name);
                    }

                    ViewData["Instruments"] = lst;
                    ViewData["id"] = user.UserId;
                    return View("CreateProfile");
                }


            } else
            {
                // If there is an issue with the login data
                return RedirectToAction("Login");
            }

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(-10);
            option.IsEssential = true;
            Response.Cookies.Append(CookieUserId, "", option);

            return RedirectToAction("Index");
        }

        public IActionResult Profile(int? id)
        {
            /* This action method displays the profile for the user with 
             *  the provided user id. Here the users should be able to 
             *  navagate to the following:
             *      - Profile   --> the profile img on the nav bar
             *      - Ensemble  --> any ensemble img in the ensembles area
             */

            SessionModel s = GetSessionInfo(Request);

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

            HashSet<Post> posts = new HashSet<Post>();
            
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
            if (s.IsLoggedIn)
            {
                model.isOwner = s.UserID == model.Profile.UserId;
            }

            model.isLoggedIn = s.IsLoggedIn;

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

            SessionModel s = GetSessionInfo(Request);
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

            model.Posts = posts;

            model.ViewType = "ensemble";
            if (s.IsLoggedIn)
            {
                model.isOwner = s.UserID == model.Ensemble.UserId;
            }

            model.isLoggedIn = s.IsLoggedIn;

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

            SessionModel s = GetSessionInfo(Request);
            VenueModel model = new VenueModel();

            var venue = _context.Venues.Where(u => u.VenueId == id).ToList()[0];

            HashSet<Post> posts = new HashSet<Post>();
            foreach (Post post in _context.Posts.OrderByDescending(p=>p.PostId))
            {
                if (post.PosterType == "venue")
                {
                    int venueId = post.PosterIndex;
                    if (venueId == venue.VenueId)
                    {
                        Venue poster_profile = _context.Venues.Find(venueId);
                        post.Venue = poster_profile;
                        if (post.Type == "gig") {
                            post.Gig = _context.Gigs.Find(post.Ref_Id);
                        }
                        posts.Add(post);
                    }


                }
            }
            model.Posts = posts;

            model.Venue = venue;
            model.ViewType = "venue";
            if (s.IsLoggedIn)
            {
                model.isOwner = s.UserID == model.Venue.UserId;
            }

            model.isLoggedIn = s.IsLoggedIn;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string email, string password)
        {
            /* This action method creates a new user in the database
             *  and moves the user to the profile creation view.
             */

            //if email already registered
            if (_context.Users.Where(u => u.Email == email).ToList().Count() > 0) {
                ViewData["Error"] = "Email already registered";
                return View("Index");
            }

            if (ModelState.IsValid)
            {
                User user = new User();
                user.Email = email;
                string salt = CreateSalt(32);
                user.Salt = salt;
                user.Password = CreatePasswordHash(password, salt);
                _context.Add(user);
                await _context.SaveChangesAsync(); //wait until DB is saved for this result

                // Once the user is created and saved, log them in
                string encUID = user.UserId.ToString();
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(1);
                option.IsEssential = true;
                Response.Cookies.Append(CookieUserId, encUID, option);

                SessionModel s = GetSessionInfo(Request);


                ProfileModel model = new ProfileModel();

                model.Instruments = new List<SelectListItem>();
                model.SelectedInsIds = new List<String>();
                foreach (Instrument i in _context.Instruments.ToList())
                {
                    var ins_name = i.Instrument_Name;
                    SelectListItem chk_ins = new SelectListItem { Text = i.Instrument_Name, Value = i.InstrumentId.ToString() };
                    model.Instruments.Add(chk_ins);
                }
                ViewData["id"] = user.UserId;
                return View("CreateProfile", model);
            }

            return View("index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProfile(string pName, string pSurname, string pCity, string pState, string pBio, int userID, ProfileModel model)
        {
            if (ModelState.IsValid)
            {
                SessionModel s = GetSessionInfo(Request);

                if (s.IsLoggedIn)
                {
                    Profile profile = new Profile();

                    profile.First_Name = pName;
                    profile.Last_Name = pSurname;             
                    profile.City = pCity;
                    profile.State = pState;
                    profile.Bio = pBio;
                    profile.User = _context.Users.Find(userID);

                    _context.Add(profile);
                    await _context.SaveChangesAsync();

                    Console.WriteLine("***");
                    profile.Plays_Instrument = new List<Plays_Instrument>();

                    foreach (String ins in model.SelectedInsIds)
                    {

                        Plays_Instrument pi = new Plays_Instrument();
                        pi.Profile = profile;
                        pi.ProfileId = pi.Profile.ProfileId;
                        pi.Instrument = _context.Instruments.Find(int.Parse(ins));
                        pi.InstrumentId = int.Parse(ins);

                        profile.Plays_Instrument.Add(pi);
                    }
                    await _context.SaveChangesAsync();

                    if (model.File != null)
                    {
                        string fileName = model.File.FileName.GetHashCode().ToString() + "." + model.File.FileName.Substring(model.File.FileName.Length - 3);
                        var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "images/uploads");
                        var filePath = Path.Combine(uploads, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.File.CopyToAsync(fileStream);
                        }
                        profile.Pic_Url = "/images/uploads/" + fileName;


                        await _context.SaveChangesAsync();
                    }

                    Console.WriteLine("***");

                    return RedirectToAction("Profile", new { id = profile.ProfileId });
                }

                // If not logged in
                string encPA = "/Home/CreateProfile";

                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(1);
                option.IsEssential = true;
                Response.Cookies.Append(CookiePrevAct, encPA, option);

                return RedirectToAction("Login");
            }

            // If invalid state
            return View("CreateProfile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEnsemble(string eName, System.DateTime eFormed, System.DateTime eDisbanded, string eCity, string eBio, string eState, string eType, string eGenre, int userID, ProfileModel model)
        {

            if (ModelState.IsValid)
            {
                SessionModel s = GetSessionInfo(Request);

                if (s.IsLoggedIn)
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

                    if (model.File != null)
                    {
                        string fileName = model.File.FileName.GetHashCode().ToString() + "." + model.File.FileName.Substring(model.File.FileName.Length - 3);
                        var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "images/uploads");
                        var filePath = Path.Combine(uploads, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.File.CopyToAsync(fileStream);
                        }
                        ensemble.Pic_Url = "/images/uploads/" + fileName;


                        await _context.SaveChangesAsync();
                    }


                    if (ModelState.IsValid)
                    {
                        _context.Add(ensemble);
                        await _context.SaveChangesAsync();
                    }

                   

                    return RedirectToAction("Ensemble", new { id = ensemble.EnsembleId });
                }
                // If not logged in
                string encPA = "/Home/CreateProfile";

                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(1);
                option.IsEssential = true;
                Response.Cookies.Append(CookiePrevAct, encPA, option);

                return RedirectToAction("Login");

            }
            // If not a valid model state
            return View("CreateProfile");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEnsembleModal(string eName, System.DateTime eFormed, System.DateTime? eDisbanded, string eCity, string eBio, string eState, string eType, string eGenre, int userID)
        {
            SessionModel s = GetSessionInfo(Request);

            if (s.IsLoggedIn)
            {
                Ensemble ensemble = new Ensemble();
                ensemble.Ensemble_Name = eName;
                ensemble.Formed_Date = eFormed;
                if (eDisbanded!= null)
                {
                    ensemble.Disbanded_Date = (System.DateTime) eDisbanded;
                }                
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

                try
                {
                    Profile profile = _context.Profiles.Where(p => p.UserId == userID).First();
                    if (profile != null)
                    {
                        Console.WriteLine("ITS HAPPENING");
                        ProfileEnsemble membership = new ProfileEnsemble();
                        membership.EnsembleId = ensemble.EnsembleId;
                        membership.ProfileId = profile.ProfileId;
                        membership.Start_Date = ensemble.Formed_Date;
                        _context.Add(membership);
                        await _context.SaveChangesAsync();
                    }
                }
                catch { }
                Console.WriteLine("***");
                Console.WriteLine(ensemble.EnsembleId);
                Console.WriteLine("***");

                return RedirectToAction("Ensemble", new { id = ensemble.EnsembleId });
            }
            // If not logged in
            string encPA = "/";

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(1);
            option.IsEssential = true;
            Response.Cookies.Append(CookiePrevAct, encPA, option);

            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVenue(string vName, string vAddr1, string vAddr2, string vCity, string vState, string vPhone, string vWeb, string vBio, int userID, ProfileModel model)
        {
            if (ModelState.IsValid)
            {
                SessionModel s = GetSessionInfo(Request);

                if (s.IsLoggedIn)
                {
                    Venue venue = new Venue();
                    venue.Venue_Name = vName;
                    venue.Address1 = vAddr1;
                    venue.Address2 = vAddr2;
                    venue.City = vCity;
                    venue.State = vState;
                    venue.Bio = vBio;
                    venue.Website = vWeb;
                    venue.Phone = vPhone;
                    venue.User = _context.Users.Find(userID);

                    if (model.File != null)
                    {
                        string fileName = model.File.FileName.GetHashCode().ToString() + "." + model.File.FileName.Substring(model.File.FileName.Length - 3);
                        var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "images/uploads");
                        var filePath = Path.Combine(uploads, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.File.CopyToAsync(fileStream);
                        }
                        venue.Pic_Url = "/images/uploads/" + fileName;


                        await _context.SaveChangesAsync();
                    }

                    _context.Venues.Add(venue);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Venue", new { id = venue.VenueId });
                }
                // If not logged in
                string encPA = "/Home/CreateProfile";

                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(1);
                option.IsEssential = true;
                Response.Cookies.Append(CookiePrevAct, encPA, option);

                return RedirectToAction("Login");
            }
            // If ModelState is invalid
            return View("Create");

        }

        public async Task<IActionResult> Audition(int id)
        {
            SessionModel s = GetSessionInfo(Request);

            if (s.IsLoggedIn)
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

            // If not logged in
            string encPA = "/Home/Audition/" + id.ToString();

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(1);
            option.IsEssential = true;
            Response.Cookies.Append(CookiePrevAct, encPA, option);
            
            return RedirectToAction("Login");
            
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyAudition(AuditionModel model) {
            SessionModel s = GetSessionInfo(Request);

            if (s.IsLoggedIn) {
                AuditionProfile application = new AuditionProfile();
                application.AuditionId = model.Audition.AuditionId;
                
                Profile profile = _context.Profiles.Where(p => p.UserId == s.UserID).First();
                application.ProfileId = profile.ProfileId;
                _context.Add(application);
                await _context.SaveChangesAsync();
               

                return RedirectToAction("Audition", new { id = model.Audition.AuditionId });

            }

            string encPA = "/Home/Audition/" + model.Audition.AuditionId.ToString();

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(1);
            option.IsEssential = true;
            Response.Cookies.Append(CookiePrevAct, encPA, option);

            return RedirectToAction("Login");

        }

        public async Task<IActionResult> Gig(int id)
        {
            SessionModel s = GetSessionInfo(Request);

            if (s.IsLoggedIn)
            {
                GigModel model = new GigModel();

                var gig = _context.Gigs.Where(g => g.GigId == id).ToList()[0];
                var ven = _context.Venues.Where(v => v.VenueId == gig.VenueId).ToList()[0];

                model.Gig = gig;
                model.Venue = ven;
                model.ViewType = "venue";

                return View(model);

            }

            // If not logged in
            string encPA = "/Home/Gig/" + id.ToString();

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(1);
            option.IsEssential = true;
            Response.Cookies.Append(CookiePrevAct, encPA, option);
            
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAudition(System.DateTime audition_date, System.DateTime closed_date, string location, string description, string eCity, int userID, int ensId, int selectedInsId)
        {

            if (ModelState.IsValid)
            {
                SessionModel s = GetSessionInfo(Request);

                if (s.IsLoggedIn)
                {
                    Audition audition = new Audition();
                    audition.Open_Date = audition_date;
                    audition.Closed_Date = closed_date;
                    audition.Audition_Location = location;
                    audition.Audition_Description = description;
                    audition.EnsembleId = ensId;
                    audition.Instrument = _context.Instruments.Find(selectedInsId);
                    audition.Instrument_Name = audition.Instrument.Instrument_Name;

                    Post post = new Post();
                    post.Text = description;
                    post.PosterType = "ensemble";
                    post.PosterIndex = ensId;
                    post.Type = "aud";                  


                    _context.Add(audition);                    
                    await _context.SaveChangesAsync();

                    //can't get the audition id until audition is saved to the database
                    post.Ref_Id = audition.AuditionId;
                    _context.Add(post);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Ensemble", new { id = ensId });
                }

                // If not logged in
                string encPA = "/Home/Ensemble/" + ensId.ToString();

                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(1);
                option.IsEssential = true;
                Response.Cookies.Append(CookiePrevAct, encPA, option);
                
                return RedirectToAction("Login");

            }

            // If ModelState is not valid
            return RedirectToAction("Index");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGig(System.DateTime start_date, System.DateTime end_date, System.TimeSpan time, string repeat, string genre, string description, int userID, int PosterIndex)
        {            
            SessionModel s = GetSessionInfo(Request);

            if (s.IsLoggedIn)
            {
                Console.WriteLine("***");
                Gig gig = new Gig();
                gig.Gig_Date = start_date + time;
                Console.WriteLine(end_date);
                gig.Closed_Date = end_date;
                gig.Genre = genre;
                gig.Description = description;
                gig.Venue = _context.Venues.Find(PosterIndex);
                if (repeat == "Yes")
                {
                    gig.Description = gig.Description + "\n This is a repeating gig";
                }

                Post post = new Post();
                post.Text = description;
                post.PosterType = "venue";
                post.PosterIndex = PosterIndex;
                post.Type = "gig";


                _context.Add(gig);
                await _context.SaveChangesAsync();

                //can't get the audition id until audition is saved to the database
                post.Ref_Id = gig.GigId;
                _context.Add(post);
                await _context.SaveChangesAsync();

                return RedirectToAction("Venue", new { id = PosterIndex });
            }

            // If not logged in
            string encPA = "/Home/Venue/" + PosterIndex.ToString();

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(1);
            option.IsEssential = true;
            Response.Cookies.Append(CookiePrevAct, encPA, option);
            
            return RedirectToAction("Login");

        }

        public async Task<IActionResult> EditProfile(int? id)
        {
            /* This action method displays view for editing the profile
             *  with the provided id. Here the users should be able to 
             *  navagate to the following:
             *      - Profile   --> nav bar
             *      - Edit[Post]--> submitting changes to the page
             */

            SessionModel s = GetSessionInfo(Request);

            if (s.IsLoggedIn)
            {
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

            return RedirectToAction("Login");
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(int id, ProfileModel model)
        {
            /* This action method takes the updated info of the user's
             *  profile and saves it. There is no view associated with
             *  this method and the user should be redirected to their
             *  respective profile page.
             */

            SessionModel s = GetSessionInfo(Request);

            if (s.IsLoggedIn)
            {
                Console.WriteLine(model.Profile.ProfileId);
                Profile userprofile = _context.Profiles.Find(model.Profile.ProfileId);
                Console.WriteLine(userprofile.ProfileId);
                userprofile.First_Name = model.Profile.First_Name;
                userprofile.Last_Name = model.Profile.Last_Name;
                userprofile.City = model.Profile.City;
                userprofile.State = model.Profile.State;
                userprofile.Bio = model.Profile.Bio;

                //handle uploading the image file to our directory
                //Console.WriteLine(model.File.FileName);

                if (model.File != null)
                {
                    string fileName = model.File.FileName.GetHashCode().ToString() + "." + model.File.FileName.Substring(model.File.FileName.Length - 3);
                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "images/uploads");
                    var filePath = Path.Combine(uploads, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.File.CopyToAsync(fileStream);
                    }
                    userprofile.Pic_Url = "/images/uploads/" + fileName;


                    await _context.SaveChangesAsync();
                }
                
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

                return RedirectToAction("Profile", new { id = model.Profile.ProfileId });
            }

            return RedirectToAction("Login");

        }

        public async Task<IActionResult> EditEnsemble(int? id) {
            SessionModel s = GetSessionInfo(Request);

            if (s.IsLoggedIn)
            {
                EnsembleModel model = new EnsembleModel();

                if (id == null)
                {
                    return NotFound();
                }

                var ensemble = await _context.Ensembles.FindAsync(id);

                if (ensemble == null)
                {
                    return NotFound();
                }

                model.Ensemble = ensemble;

                return View(model);
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEnsemble(int id, EnsembleModel model) {
            SessionModel s = GetSessionInfo(Request);

            if (s.IsLoggedIn)
            {
                Ensemble ensemble = _context.Ensembles.Find(model.Ensemble.EnsembleId);
                ensemble.Ensemble_Name = model.Ensemble.Ensemble_Name;
                ensemble.City = model.Ensemble.City;
                ensemble.State = model.Ensemble.State;
                ensemble.Genre = model.Ensemble.Genre;
                ensemble.Type = model.Ensemble.Type;
                ensemble.Bio = model.Ensemble.Bio;
                ensemble.Formed_Date = model.Ensemble.Formed_Date;
                ensemble.Disbanded_Date = model.Ensemble.Disbanded_Date;

                //handle uploading the image file to our directory
                //Console.WriteLine(model.File.FileName);

                if (model.File != null)
                {
                    string fileName = model.File.FileName.GetHashCode().ToString() + "." + model.File.FileName.Substring(model.File.FileName.Length - 3);
                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "images/uploads");
                    var filePath = Path.Combine(uploads, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.File.CopyToAsync(fileStream);
                    }
                    ensemble.Pic_Url = "/images/uploads/" + fileName;
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Ensemble", new { id = model.Ensemble.EnsembleId });
            }

            return RedirectToAction("Login");
        }

        
        public async Task<IActionResult> EditVenue(int? id) {
            SessionModel s = GetSessionInfo(Request);

            if (s.IsLoggedIn)
            {
                VenueModel model = new VenueModel();

                if (id == null)
                {
                    return NotFound();
                }

                var venue = await _context.Venues.FindAsync(id);

                if (venue== null)
                {
                    return NotFound();
                }

                model.Venue = venue;

                return View(model);
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVenue(int id, VenueModel model) {
            SessionModel s = GetSessionInfo(Request);

            if (s.IsLoggedIn)
            {
                Venue venue = _context.Venues.Find(model.Venue.VenueId);
                venue.Venue_Name = model.Venue.Venue_Name;
                venue.City = model.Venue.City;
                venue.State = model.Venue.State;
                venue.Address1 = model.Venue.Address1;
                venue.Address2 = model.Venue.Address2;
                venue.Bio = model.Venue.Bio;
                venue.Website = model.Venue.Website;
                venue.Phone = model.Venue.Phone;

                //handle uploading the image file to our directory
                //Console.WriteLine(model.File.FileName);

                if (model.File != null)
                {
                    string fileName = model.File.FileName.GetHashCode().ToString() + "." + model.File.FileName.Substring(model.File.FileName.Length - 3);
                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "images/uploads");
                    var filePath = Path.Combine(uploads, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.File.CopyToAsync(fileStream);
                    }
                    venue.Pic_Url = "/images/uploads/" + fileName;
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Venue", new { id = model.Venue.VenueId });
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> createPost([Bind("Text, PosterType, PosterIndex, Type")] Post post, PageModel model, string PosterType, int PosterIndex)
        {

            if (ModelState.IsValid)
            {
                SessionModel s = GetSessionInfo(Request);

                if (s.IsLoggedIn)
                {
                    //handle uploading the image file to our directory
                    //Console.WriteLine(model.File.FileName)

                    if (model.File != null)
                    {
                        string fileName = model.File.FileName.GetHashCode().ToString() + "." + model.File.FileName.Substring(model.File.FileName.Length - 3);
                        var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "images/uploads");
                        var filePath = Path.Combine(uploads, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.File.CopyToAsync(fileStream);
                        }
                        post.MediaUrl = "/images/uploads/" + fileName;

                        //parsing media type
                        HashSet<string> img_extensions = new HashSet<string>();
                        HashSet<string> audio_extensions = new HashSet<string>();
                        HashSet<string> video_extensions = new HashSet<string>();
                        HashSet<string> resume_extensions = new HashSet<string>();
                        img_extensions.Add("png");
                        img_extensions.Add("jpg");
                        img_extensions.Add("gif");
                        audio_extensions.Add("mp3");
                        video_extensions.Add("mp4");
                        video_extensions.Add("mov");
                        resume_extensions.Add("pdf");
                        if (img_extensions.Contains(fileName.Substring(fileName.Length - 3)))
                        {
                            post.MediaType = "img";
                        }
                        if (audio_extensions.Contains(fileName.Substring(fileName.Length - 3)))
                        {
                            post.MediaType = "audio";
                        }
                        if (video_extensions.Contains(fileName.Substring(fileName.Length - 3)))
                        {
                            post.MediaType = "video";
                        }
                        if (resume_extensions.Contains(fileName.Substring(fileName.Length - 3)))
                        {
                            post.MediaType = "resume";
                        }
                    }


                    //add the post to database
                    _context.Add(post);
                    await _context.SaveChangesAsync();
                    if (PosterType == "profile")
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
                // If not logged in
                return RedirectToAction("Login");
            }
            // If invalid model state
            //This is bad !!! X.X
            return View("Index");

        }
        
        public IActionResult Dashboard(int id)
        {
            SessionModel s = GetSessionInfo(Request);

            if (s.IsLoggedIn)
            {
                Ensemble ensemble = _context.Ensembles.Where(u => u.EnsembleId == id).First();

                if (s.UserID == ensemble.UserId)
                {
                    EnsembleDashModel model = new EnsembleDashModel();

                    model.Ensemble = ensemble;
                    model.AuditionList = _context.Auditions.Where(u => u.EnsembleId == id && u.Closed_Date > System.DateTime.Now).ToList();
                    model.Members = _context.ProfileEnsembles.Include("Profile").Where(pe => pe.EnsembleId == ensemble.EnsembleId).ToList();
       
                    return View(model);
                }

                return RedirectToAction("Ensemble", new { id = id });
            }

            return RedirectToAction("Login");

        }
        
         public IActionResult Search(string type, string query)
        {
            Console.WriteLine("***");
            Console.WriteLine(query);
            Console.WriteLine("***");
            SearchModel model = new SearchModel();


            HashSet<Audition> audresult = new HashSet<Audition>();
            HashSet<Gig> gigresult = new HashSet<Gig>();
            HashSet<Profile> profileresult = new HashSet<Profile>();
            HashSet<Ensemble> ensembleresult = new HashSet<Ensemble>();
            HashSet<Venue> venueresult = new HashSet<Venue>();

            string[] wordList = query.Split();
            foreach (String word in wordList)
            {
                //Auditions--find by instrument
                List<Audition> auditions = _context.Auditions.Where(a => a.Instrument_Name == word).ToList();
                foreach (Audition aud in auditions)
                {
                    audresult.Add(aud);
                }
                //Gigs--find by genre
                List<Gig> gigs = _context.Gigs.Where(g => g.Genre == word || g.Genre ==query).ToList();
                foreach (Gig gig in gigs)
                {
                    gigresult.Add(gig);
                }
                //Profiles--find by name, instrument
                List<Profile> profiles = _context.Profiles.Where(p => p.First_Name == word || p.Last_Name == word).ToList();
                foreach (Profile profile in profiles)
                {
                    profileresult.Add(profile);
                }
                List<Plays_Instrument> profByInstr = _context.Plays_Instruments.Where(pi => pi.Instrument.Instrument_Name == word).ToList();
                foreach (Plays_Instrument pi in profByInstr)
                {
                    profileresult.Add(_context.Profiles.Find(pi.ProfileId));
                }
                //Ensembles--find by name, genre
                List<Ensemble> ensembles = _context.Ensembles.Where(e => e.Ensemble_Name == word || e.Ensemble_Name == query || e.Genre == word || e.Genre == query || e.Type == word || e.Type == query).ToList();
                foreach (Ensemble e in ensembles)
                {
                    ensembleresult.Add(e);
                }
                //Venues--find by name
                List<Venue> venues = _context.Venues.Where(v => v.Venue_Name == word || v.Venue_Name == query).ToList();
                foreach(Venue v in venues)
                {
                    venueresult.Add(v);
                }
            }

            model.Auditions = audresult;
            model.AuditionCount = audresult.Count();
            model.Gigs = gigresult;
            model.GigCount = gigresult.Count();
            model.Profiles = profileresult;
            model.ProfileCount = profileresult.Count();
            model.Ensembles = ensembleresult;
            model.EnsembleCount = ensembleresult.Count();
            model.Gigs = gigresult;
            model.GigCount = gigresult.Count();
            model.Venues = venueresult;
            model.VenueCount = venueresult.Count();
            model.Query = query;
         
            return View(model);
        }

    }
}
