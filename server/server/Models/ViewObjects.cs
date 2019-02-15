using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace server.Models
{
    public class ProfileModel
    {
        //the profile we are displaying
        public Profile Profile { get; set; }
        //the image, its relative file path is stored in Profile.Pic_Url
        public IFormFile File { get; set; }
        //List of the Profile's Ensembles
        public List<Ensemble> Ensembles { get; set; } 
        public List<SelectListItem> Instruments { get; set; }
        public List<String> SelectedInsIds { get; set; }
        //Current user
        public User User { get; set; }  
        //Stores "profile", "ensemble", or "venue" so our View knows how to format
        public String ViewType { get; set; }
        //whether or not you own this page
        public Boolean isOwner { get; set; }
        //whether or not you are logged in
        public Boolean isLoggedIn { get; set; }

        // For the sake of our MVP demonstration, this is how we will load posts
        public List<Post> Posts { get; set; }
    }

    public class EnsembleModel
    {
        //the profile we are displaying
        public Ensemble Ensemble { get; set; }
        //List of the Profile's Ensembles
        public List<Profile> Profiles { get; set; }
        //Current user
        public List<SelectListItem> Instruments { get; set; }
        public List<String> SelectedInsId { get; set; }
        //instrument that ensemble selected for audition
        //all possible instruments
        public User User { get; set; }
        //Stores "profile", "ensemble", or "venue" so our View knows how to format
        public String ViewType { get; set; }
        //whether or not you own this page
        public Boolean isOwner { get; set; }
        //whether or not you are logged in
        public Boolean isLoggedIn { get; set; }
        // For the sake of our MVP demonstration, this is how we will load posts
        public HashSet<Post> Posts { get; set; }
        public ICollection<Audition> Audition { get; set; }

    }

    public class VenueModel
    {
        //the venue begin displayed
        public Venue Venue { get; set; }
        //Current user
        public User User { get; set; }
        //whether or not you own this page
        public Boolean IsOwner { get; set; }
        //whether or not you are logged in
        public Boolean isLoggedIn { get; set; }
        //Stores "profile", "ensemble", or "venue" so our View knows how to format
        public String ViewType { get; set; }

        // For the sake of our MVP demonstration, this is how we will load posts
        public List<Post> Posts { get; set; }

    }

    public class AuditionModel
    {
        public Ensemble Ensemble { get; set; }

        public Audition Audition { get; set; }
        //Stores "profile", "ensemble", or "venue" so our View knows how to format
        public String ViewType { get; set; }

        public List<Profile> Profiles { get; set; }

    }

    public class AuditionSearch
    {
        public List<Audition> Auditions { get; set; }
    }

    public class GigModel
    {
        //Stores "profile", "ensemble", or "venue" so our View knows how to format
        public String ViewType { get; set; }
        public Venue venue { get; set; }
        public Gig gig { get; set; }
    }

    public class SessionModel
    {
        /* This is not a view object, but rather a container for data 
         * that is stored in HttpContext.Session.
         */
        
        public int UserID { get; set; }
        //public String ViewType { get; set; }
        //public int ViewID { get; set; }
        public bool IsLoggedIn { get; set; }
        public string PrevAction { get; set; }
    }
}