using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using server.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace server.Controllers
{
    [Route("api/PluggedAPI")]
    [ApiController]
    public class PluggedAPIController : ControllerBase
    {
        private readonly PluggedContext _context;

        public PluggedAPIController(PluggedContext context)
        {
            _context = context;
        }

        // GET: api/PluggedAPI/posts
        [HttpGet("posts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            return await _context.Posts.ToListAsync();
        }


        //Load all of the Profiles who have applied for a given audition (id)
        [HttpGet("applicants/{id}")]
        public async Task<ActionResult<IEnumerable<Profile>>> GetApplicants(int id)
        {
            var auditionprofile = _context.AuditionProfiles.Where(a => a.AuditionId == id).ToList();

            var applicantProfiles = new List<Profile>();

            foreach (AuditionProfile elem in auditionprofile)
            {
                applicantProfiles.Add(_context.Profiles.Find(elem.ProfileId));
            }

            return applicantProfiles;
        }


        [HttpGet("members/{id}")]
        public async Task<ActionResult<IEnumerable<ProfileEnsemble>>> GetMembers(int id)
        {
            var memberProfiles = _context.ProfileEnsembles.Include("Profile").Where(pe => pe.EnsembleId == id).ToList();

            return memberProfiles;
        }

        [HttpPost("transferOwner")]
        public async Task<ActionResult<TransferModel>> transferOwner(TransferOwner murderChildren)
        {
            var profiles = _context.Profiles.Include("User").Where(p => p.User.Email == murderChildren.Email).ToList();

            /* If there are more than one person with the same first and last name,
             * for the scope of this project, we will just say that you cannot transfer
             * ensemble ownership due to the uncertainty of who you are transfering to.
             * Additionally, if there are no matches we will also not allow them to
             * transfer ownership.
             * 
             * This is not the ideal solution, but for the scope of our project, this is
             * the solution we are going with.
             * 
             * The ideal solution would be to give the user a list of all the matching 
             * results and allow them to select which one they would like to transfer to.
             * 
             */

            var retModel = new TransferModel();

            if (profiles.Count < 1)
            {
                retModel.Transferred = false;
                retModel.Email = murderChildren.Email;

                return retModel;
            }

            var ensemble = _context.Ensembles.Where(e => e.EnsembleId == murderChildren.EnsembleId).First();
            ensemble.UserId = profiles[0].UserId;
            await _context.SaveChangesAsync();

            retModel.Transferred = true;
            retModel.Email = murderChildren.Email;

            return retModel;

        }

        //Display all information about a given audition (id)
        [HttpGet("auditions/{id}")]
        public async Task<ActionResult<Audition>> GetAudition(int id)
        {
            var audition = await _context.Auditions.FindAsync(id);

            if (audition == null)
            {
                return NotFound();
            }

            return audition;
        }

        public class ChangeAudition
        {
            public string auditionId { get; set; }
            public string audition_Description { get; set; }
            public string audition_Location { get; set; }
            public string closed_Date { get; set; }
            public string instrument_Name { get; set; }
            public string open_Date { get; set; }
        }

        //Change any information about a given audition (id)
        [HttpPost("auditions/{id}")]
        public async Task<IActionResult> PostAudition(int id, ChangeAudition aud)
        {
            var audition = _context.Auditions.Find(id);
            audition.Audition_Description = aud.audition_Description;
            audition.Audition_Location = aud.audition_Location;
            audition.Closed_Date = DateTime.ParseExact(aud.closed_Date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            audition.Open_Date = DateTime.ParseExact(aud.open_Date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            var inst = _context.Instruments.Where(p => p.Instrument_Name == aud.instrument_Name).ToList()[0];
            audition.InstrumentId = inst.InstrumentId;
            audition.Instrument_Name = inst.Instrument_Name;

            //_context.Entry(aud).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();

        }


        //Close a given audition (id) by setting its closed date to now
        [HttpGet("closeAud/{id}")]
        public async Task<IActionResult> CloseAudition(int id)
        {

            var audition = await _context.Auditions.FindAsync(id);

            audition.Closed_Date = System.DateTime.Now;

            await _context.SaveChangesAsync();


            return NoContent();

        }

        //Open a given audition (id) by setting its closed date to 30 days from now
        [HttpGet("openAud/{id}")]
        public async Task<IActionResult> OpenAudition(int id)
        {

            var audition = await _context.Auditions.FindAsync(id);


            System.DateTime today = System.DateTime.Now;
            System.TimeSpan duration = new System.TimeSpan(30, 0, 0, 0);
            audition.Closed_Date = today.Add(duration);

            await _context.SaveChangesAsync();

            return NoContent();

        }

        public class GetMembersHelper
        {
            public int id { get; set; }
        }

        public class AddProfiletoEnsemble
        {
            public string name { get; set; }
            public string EnsembleId { get; set; }
        }

        public class AddProfiletoAudition
        {
            public string ProfileId { get; set; }
            public string AuditionId { get; set; }
        }

        public class RemoveProfileEnsemble
        {
            public string ProfileId { get; set; }
            public string EnsembleId { get; set; }
        }

        public class AcceptApplicant
        {
            public string AuditionId { get; set; }
            public string ProfileId { get; set; }
            public string EnsembleId { get; set; }
        }

        public class RemoveApplicant
        {
            public string AuditionId { get; set; }
            public string ProfileId { get; set; }
        }

        public class TransferOwner
        {
            public string Email { get; set; }
            public int EnsembleId { get; set; }
        }

        public class TransferModel
        {
            public bool Transferred { get; set; }
            public string Email { get; set; }
        }

        //Add a profile to an ensemble
        [HttpPost("addProfile")]
        public async Task<IActionResult> addProfile(AddProfiletoEnsemble addition)
        {
            var name = addition.name.Split();
            var Profiles = _context.Profiles.Where(p => p.First_Name == name[0] && p.Last_Name == name[1]).ToList();

            foreach (Profile profile in Profiles)
            {
                ProfileEnsemble profens = new ProfileEnsemble();
                profens.Start_Date = System.DateTime.Now;
                profens.End_Date = System.DateTime.MaxValue;
                profens.ProfileId = profile.ProfileId;
                profens.Profile = profile;
                profens.Ensemble = _context.Ensembles.Find(int.Parse(addition.EnsembleId));
                profens.EnsembleId = int.Parse(addition.EnsembleId);

                if (ModelState.IsValid)
                {
                    _context.Add(profens);
                    await _context.SaveChangesAsync();
                }

            }

            return NoContent();

        }

        //Add a profile to an audition
        [HttpPost("addApplicant")]
        public async Task<IActionResult> addApplicant(AddProfiletoAudition addition)
        {

            AuditionProfile audprof = new AuditionProfile();
            audprof.AuditionId = int.Parse(addition.AuditionId);
            audprof.Audition = _context.Auditions.Find(int.Parse(addition.AuditionId));
            audprof.ProfileId = int.Parse(addition.ProfileId);
            audprof.Profile = _context.Profiles.Find(int.Parse(addition.ProfileId));

            if (ModelState.IsValid)
            {
                _context.Add(audprof);
                await _context.SaveChangesAsync();
            }

            return NoContent();

        }



        //Accept a profile from an audition into an ensemble.
        [HttpPost("acceptApplicant")]
        public async Task<IActionResult> acceptApplicant(AcceptApplicant applicant)
        {


            var alreadyMember = _context.ProfileEnsembles.Where(p => p.EnsembleId == int.Parse(applicant.EnsembleId) && p.ProfileId == int.Parse(applicant.ProfileId)).ToList();

            if (alreadyMember.Count() >= 1)
            {
                return NoContent();
            }

            var profile = _context.Profiles.Find(int.Parse(applicant.ProfileId));

            ProfileEnsemble profens = new ProfileEnsemble();
            profens.Start_Date = System.DateTime.Now;
            profens.End_Date = System.DateTime.MaxValue;
            profens.ProfileId = profile.ProfileId;
            profens.Profile = profile;
            profens.Ensemble = _context.Ensembles.Find(int.Parse(applicant.EnsembleId));
            profens.EnsembleId = int.Parse(applicant.EnsembleId);

            if (ModelState.IsValid)
            {
                _context.Add(profens);
                await _context.SaveChangesAsync();
            }

            return NoContent();

        }

        //get all open auditions for a particular ensemble
        [HttpGet("getOpenAuditions/{id}")]
        public async Task<ActionResult<IEnumerable<Audition>>> getOpenAuditions(int id)
        {
            var auditionlist = _context.Auditions.Where(a => a.EnsembleId == id && a.Closed_Date > System.DateTime.Now).ToList();
            return auditionlist;
        }

        //get all closed auditions for a particular ensemble
        [HttpGet("getClosedAuditions/{id}")]
        public async Task<ActionResult<IEnumerable<Audition>>> getClosedAuditions(int id)
        {
            var auditionlist = _context.Auditions.Where(a => a.EnsembleId == id && a.Closed_Date <= System.DateTime.Now).ToList();
            return auditionlist;
        }

        //get all auditions for a particular ensemble
        [HttpGet("getAllAuditions/{id}")]
        public async Task<ActionResult<IEnumerable<Audition>>> getAllAuditions(int id)
        {
            var auditionlist = _context.Auditions.Where(a => a.EnsembleId == id).ToList();
            return auditionlist;
        }

        //Remove a profile from an ensemble.
        [HttpPost("remProfile")]
        public async Task<IActionResult> removeProfile(RemoveProfileEnsemble rm)
        {
            var prof_ens = _context.ProfileEnsembles.Where(p => p.EnsembleId == int.Parse(rm.EnsembleId) && p.ProfileId == int.Parse(rm.ProfileId)).ToList()[0];
            _context.Remove(prof_ens);
            await _context.SaveChangesAsync();

            return NoContent();

        }


        //Remove an applicant from an audition.
        [HttpPost("remApplicant")]
        public async Task<IActionResult> removeApplicant(RemoveApplicant rm)
        {
            var aud_prof = _context.AuditionProfiles.Where(p => p.AuditionId == int.Parse(rm.AuditionId) && p.ProfileId == int.Parse(rm.ProfileId)).ToList()[0];
            _context.Remove(aud_prof);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        // GET: api/PluggedAPI/posts/5
        [HttpGet("posts/{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        /*
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        */


        // POST api/PluggedAPI/posts
        [HttpPost("posts")]
        public async Task<ActionResult<Post>> Postpost(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPost), new { id = post.PostId }, post);
        }
        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}