using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace pluggedIn_entity.Models
{
    /*
    public class BloggingContext : DbContext
    {
        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options)
        { }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }

        public ICollection<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
    */

    public class PluggedContext : DbContext
    {
        public PluggedContext(DbContextOptions<PluggedContext> options)
            : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }

    }

    public class User
    {
        public int UserId { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<Profile> Profile { get; set; }
        public ICollection<Ensemble> Ensemble { get; set; }
        public ICollection<Venue> Venue { get; set; }
    }

    public class Profile
    {
        public int ProfileId { get; set; }

        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Preferred_Name { get; set; }
        public string Pic_Url { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Plays_Instrument> Plays_Instrument { get; set; }
        //public ICollection<Ensemble_Membership> Ensemble_Membership { get; set; }
    }

    public class Ensemble
    {
        public int EnsembleId { get; set; }

        public string Ensemble_Name { get; set; }
        public System.DateTime Formed_Date { get; set; }
        public System.DateTime Disbanded_Date { get; set; }
        public string Type { get; set; }
        public string Genre { get; set; }
        public string Pic_Url { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        //public ICollection<Ensemble_Membership> Ensemble_Membership { get; set; }
        public ICollection<Audition> Audition { get; set; }
    }

    public class Venue
    {
        public int VenueId { get; set; }

        public string Venue_Name { get; set; }
        public string Location { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Gig> Gig { get; set; }
    }

    public class Instrument
    {
        public int InstrumentId { get; set; }
        public string Instrument_Name { get; set; }

        public ICollection<Plays_Instrument> Plays_Instrument { get; set; }
    }

    public class Plays_Instrument
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int InstrumentId { get; set; }
        public Instrument Instrument { get; set; }
    }
    /*
    public class Ensemble_Membership
    {
        public int Id { get; set; }
        public System.DateTime Start_Date { get; set; }
        public System.DateTime End_Date { get; set; }

        public int ProfileId { get; set; }
        public User User { get; set; }

        public int EnsembleId { get; set; }
        public Ensemble Ensemble { get; set; }
    }
    */

    public class Audition
    {
        public int AuditionId { get; set; }
        public System.DateTime Open_Date { get; set; }
        public System.DateTime Closed_Date { get; set; }
        public string Audition_Location { get; set; }
        public string Audition_Description { get; set; }
        public string Instrument_Name { get; set; }

        public int EnsembleId { get; set; }
        public Ensemble Ensemble { get; set; }
    }

    public class Gig
    {
        public int GigId { get; set; }
        public System.DateTime Gig_Date { get; set; }

        public int VenueId { get; set; }
        public Venue Venue { get; set; }

        public ICollection<Booked_Gig> Booked_Gig { get; set; }
    }

    public class Booked_Gig
    {
        public int Id { get; set; }
        public System.DateTime Date_Booked { get; set; }

        public int GigId { get; set; }
        public Gig Gig { get; set; }

    }
}
