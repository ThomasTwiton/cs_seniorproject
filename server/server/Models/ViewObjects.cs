using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace server.Models
{
    public class PageModel
    {
        //the image, its relative file path is stored in Profile.Pic_Url
        public IFormFile File { get; set; }
        //Current user
        public User User { get; set; }
        //Stores "profile", "ensemble", or "venue" so our View knows how to format
        public String ViewType { get; set; }
        //whether or not you own this page
        public Boolean isOwner { get; set; }
        //whether or not you are logged in
        public Boolean isLoggedIn { get; set; }
        // For the sake of our MVP demonstration, this is how we will load posts
        public HashSet<Post> Posts { get; set; }
    }
    public class ProfileModel : PageModel
    {
        //the profile we are displaying
        public Profile Profile { get; set; }
        
        //List of the Profile's Ensembles
        public List<Ensemble> Ensembles { get; set; } 
        public List<SelectListItem> Instruments { get; set; }
        public List<String> SelectedInsIds { get; set; }        
    }

    public class EnsembleModel : PageModel
    {
        //the profile we are displaying
        public Ensemble Ensemble { get; set; }
        //List of the Profile's Ensembles
        public List<Profile> Profiles { get; set; }
       
        public List<SelectListItem> Instruments { get; set; }
        public List<String> SelectedInsId { get; set; }
        //instrument that ensemble selected for audition
        //all possible instrument
 
        public ICollection<Audition> Audition { get; set; }

    }

    public class VenueModel : PageModel
    {
        //the venue begin displayed
        public Venue Venue { get; set; }
    }

    public class AuditionModel
    {
        public Ensemble Ensemble { get; set; }

        public Audition Audition { get; set; }
        //Stores "profile", "ensemble", or "venue" so our View knows how to format
        public String ViewType { get; set; }

        public ICollection<Profile> Profiles { get; set; }

    }

    public class GigModel
    {
        //Stores "profile", "ensemble", or "venue" so our View knows how to format
        public String ViewType { get; set; }
        public Venue Venue { get; set; }
        public Gig Gig { get; set; }
    }

    public class EnsembleDashModel
    {
        public Ensemble Ensemble { get; set; }
        public List<ProfileEnsemble> Members { get; set; }

        /* The AuditionList is a list containing all of the
         * PENDING auditions for a particular ensemble. 
         * 
         * The AuditionNumbers is a dictionary where the key
         * is an Audition Id and the value is the number of 
         * applicants there are for that audition.
         */
        public List<Audition> AuditionList { get; set; }
        public Dictionary<int,int> AuditionNumbers { get; set; }
    }

    public class VenueDashModel
    {

    }

    public class SessionModel
    {
        /* This is not a view object, but rather 
         * a container for data that is stored in
         * HttpContext.Session.
         */
        
        public int UserID { get; set; }
        //public String ViewType { get; set; }
        //public int ViewID { get; set; }
        public bool IsLoggedIn { get; set; }
        public string PrevAction { get; set; }
    }

    public class SearchModel {
        public HashSet<Audition> Auditions { get; set; }
        public int AuditionCount { get; set; }
        public HashSet<Gig> Gigs { get; set; }
        public int GigCount { get; set; }
        public HashSet<Profile> Profiles { get; set; }
        public int ProfileCount { get; set; }
        public HashSet<Ensemble> Ensembles { get; set; }
        public int EnsembleCount { get; set; }
        public HashSet<Venue> Venues { get; set; }
        public int VenueCount { get; set; }
        public String Query { get; set; }
    }
}