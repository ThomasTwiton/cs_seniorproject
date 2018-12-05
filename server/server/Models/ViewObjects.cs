﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class ProfileModel
    {
        //the profile we are displaying
        public Profile Profile { get; set; }
        //List of the Profile's Ensembles
        public List<Ensemble> Ensembles { get; set; }
        public List<Instrument> Instruments { get; set; }
        //Current user
        public User User { get; set; }  
        //Stores "profile", "ensemble", or "venue" so our View knows how to format
        public String ViewType { get; set; }
        //whether or not you own this page
        public Boolean isOwner { get; set; }

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
        public User User { get; set; }
        //Stores "profile", "ensemble", or "venue" so our View knows how to format
        public String ViewType { get; set; }
        //whether or not you own this page
        public Boolean isOwner { get; set; }

        // For the sake of our MVP demonstration, this is how we will load posts
        public List<Post> Posts { get; set; }
    }

    public class VenueModel
    {
        //the venue begin displayed
        public Venue Venue { get; set; }
        //Current user
        public User User { get; set; }
        //whether or not you own this page
        public Boolean IsOwner { get; set; }
        //Stores "profile", "ensemble", or "venue" so our View knows how to format
        public String ViewType { get; set; }

        // For the sake of our MVP demonstration, this is how we will load posts
        public List<Post> Posts { get; set; }

    }

    public class AuditionModel
    {
        public Profile profile { get; set; }

        public List<Ensemble> Ensembles { get; set; }
        //Stores "profile", "ensemble", or "venue" so our View knows how to format
        public String ViewType { get; set; }

    }

    public class GigModel
    {
        //Stores "profile", "ensemble", or "venue" so our View knows how to format
        public String ViewType { get; set; }
        public Venue venue { get; set; }
        public Gig gig { get; set; }
    }
}
