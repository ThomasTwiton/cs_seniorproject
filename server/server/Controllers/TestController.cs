using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.Models;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    public class TestController : Controller
    {
        private readonly PluggedContext _context;
        public TestController(PluggedContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Audition()
        {
            var user_with_profile = _context.Users.Include(p => p.Profile).Where(u => u.Email == "conzty01@luther.edu" && u.Password == "123456").ToList()[0];
            var profile = user_with_profile.Profile.ToList()[0];
            var Ensembles = new List<Ensemble>();

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
            foreach (ProfileEnsemble pe in profile.ProfileEnsemble)
            {
                //Ensemble ensemble = pe.Ensemble.Ensemble
                //String ensembleN = pe.Ensemble.Ensemble_Name;
                //String ensembleID = pe.Ensemble.EnsembleId.ToString();
                //Console.WriteLine(ensemble);
                Ensembles.Add(pe.Ensemble);
            }

            if (profile == null)
            {
                return NotFound();
            }

            ViewData["first_name"] = profile.First_Name;
            ViewData["last_name"] = profile.Last_Name;


            /* The following are other bits of information
             * that are needed for the view. These are all
             * just templated code. */
            ViewData["Name"] = profile.First_Name + " " + profile.Last_Name;
            ViewData["Title"] = "Ringo Starr - Profile";
            ViewData["Bio"] = "English musician, singer, actor, songwriter, and drummer for the Beatles.";
            ViewData["Location"] = "Liverpool";
            ViewData["ProfPicURL"] = "https://placekitten.com/g/64/64";
            ViewData["Owner"] = "true";
            ViewData["ProfileType"] = "audition";

            ViewData["Aud_Pos"] = "Vocalist";
            ViewData["Aud_Beg"] = "August 10, 2018";
            ViewData["Aud_End"] = "December 25, 2018";
            ViewData["Aud_Loc"] = "Marty's Grill";
            ViewData["Aud_Des"] = "We are currently looking for a new vocalist. For the audition, you will want to prepare a couple of pieces that showcase your voice and what you're capable of. Keep in mind, that we do have a particular sound we are going for and you in order to be a Beatle, you will need to sound like one too.";

            return View(Ensembles);
        }

        public IActionResult Search()
        {
            ViewData["Title"] = "Search - Beatles";
            return View();
        }
    }
}