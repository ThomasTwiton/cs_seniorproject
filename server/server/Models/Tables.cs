using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace server.Models
{
    public class PluggedContext : DbContext
    {

        public PluggedContext() { }

        public PluggedContext(DbContextOptions<PluggedContext> options)
            : base(options)
        { }

        //tables in the database
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<Venue> Venues { get; set; }
        public virtual DbSet<Ensemble> Ensembles { get; set; }
        public virtual DbSet<Instrument> Instruments { get; set; }
        public virtual DbSet<Plays_Instrument> Plays_Instruments { get; set; }
        public virtual DbSet<ProfileEnsemble> ProfileEnsembles { get; set; }
        public virtual DbSet<Audition> Auditions { get; set; }
        public virtual DbSet<Gig> Gigs { get; set; }
        public virtual DbSet<Booked_Gig> Booked_Gigs { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        //public DbSet<PostMedia> PostMedias { get; set; }
        //public DbSet<E_Has_Media> E_Has_Medias { get; set; }
        //public DbSet<P_Has_Media> P_Has_Medias { get; set; }
        //public DbSet<V_Has_Media> V_Has_Medias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<ProfileEnsemble>()
                .HasKey(em => new { em.ProfileId, em.EnsembleId });

            modelBuilder.Entity<ProfileEnsemble>()
                .HasOne(em => em.Profile)
                .WithMany(b => b.ProfileEnsemble)
                .HasForeignKey(em => em.ProfileId);

            modelBuilder.Entity<ProfileEnsemble>()
                .HasOne(em => em.Ensemble)
                .WithMany(c => c.ProfileEnsemble)
                .HasForeignKey(em => em.EnsembleId);


            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // equivalent of modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
                // and modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
                entityType.GetForeignKeys()
                    .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                    .ToList()
                    .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
            }

            //seed the database (needed)
            modelBuilder.Entity<Instrument>().HasData(
                    new Instrument() { InstrumentId = 1, Instrument_Name = "Piano" },
                    new Instrument() { InstrumentId = 2, Instrument_Name = "Voice" },
                    new Instrument() { InstrumentId = 3, Instrument_Name = "Violin" },
                    new Instrument() { InstrumentId = 4, Instrument_Name = "Viola" },
                    new Instrument() { InstrumentId = 5, Instrument_Name = "Cello" },
                    new Instrument() { InstrumentId = 6, Instrument_Name = "Bass" },
                    new Instrument() { InstrumentId = 7, Instrument_Name = "Guitar" },
                    new Instrument() { InstrumentId = 8, Instrument_Name = "Drums" },
                    new Instrument() { InstrumentId = 9, Instrument_Name = "Trumpet" },
                    new Instrument() { InstrumentId = 10, Instrument_Name = "Trombone" },
                    new Instrument() { InstrumentId = 22, Instrument_Name = "Bass Trombone" },
                    new Instrument() { InstrumentId = 11, Instrument_Name = "Tuba" },
                    new Instrument() { InstrumentId = 12, Instrument_Name = "Baritone" },
                    new Instrument() { InstrumentId = 13, Instrument_Name = "French Horn" },
                    new Instrument() { InstrumentId = 14, Instrument_Name = "Flute" },
                    new Instrument() { InstrumentId = 15, Instrument_Name = "Clarinet" },
                    new Instrument() { InstrumentId = 16, Instrument_Name = "Basoon" },
                    new Instrument() { InstrumentId = 17, Instrument_Name = "Saxophone" },
                    new Instrument() { InstrumentId = 18, Instrument_Name = "Bagpipes" },
                    new Instrument() { InstrumentId = 19, Instrument_Name = "Xylophone" },
                    new Instrument() { InstrumentId = 20, Instrument_Name = "Accordion" },
                    new Instrument() { InstrumentId = 21, Instrument_Name = "Harmonica" }
                );
            //seed the database (for testing)
            modelBuilder.Entity<User>().HasData(
                new User() { UserId = 1, Email = "miley@cyrus.com", Password = "bestObothWorlds" },
                new User() { UserId = 2, Email = "billy@cyrus.com", Password = "bestDad" }
            );
            modelBuilder.Entity<Profile>().HasData(
                new Profile()
                {
                    ProfileId = 11,
                    UserId = 1,
                    First_Name = "Miley",
                    Last_Name = "Cyrus",
                    Preferred_Name = "The Wrecking Ball",
                    Pic_Url = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/170526-N-EO381-052_Miley_Cyrus_on_Today_show.jpg/330px-170526-N-EO381-052_Miley_Cyrus_on_Today_show.jpg",
                    Bio = "Miley Ray Cyrus (born Destiny Hope Cyrus; November 23, 1992) is an American singer, songwriter, and actress.",
                    City = "Los Angeles",
                    State = "Caliornia"
                },
                new Profile()
                {
                    ProfileId = 12,
                    UserId = 2,
                    First_Name = "Billy Ray",
                    Last_Name = "Cyrus",
                    Preferred_Name = "Dad",
                    Pic_Url = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4a/Billy_Ray_Cyrus_2009_%28cropped%29.jpg/330px-Billy_Ray_Cyrus_2009_%28cropped%29.jpg",
                    Bio = "William Ray Cyrus (born August 25, 1961)[1][2] is an American singer, songwriter and actor.",
                    City = "Flatwoods",
                    State = "Kentucky"
                }
                );
            modelBuilder.Entity<Ensemble>().HasData(
                new Ensemble()
                {
                    EnsembleId = 21,
                    UserId = 1,
                    Ensemble_Name = "Hannah Montana Show",
                    Formed_Date = new System.DateTime(2006, 3, 1),
                    Type = "TV Show",
                    Genre = "Pop",
                    City = "Disneyworld",
                    State = "Disney",
                    Bio = "Is it really the best of both worlds?",
                    Pic_Url = "https://upload.wikimedia.org/wikipedia/en/2/2b/Hannah_Montana_Logo.PNG"
                },
                new Ensemble()
                {
                    EnsembleId = 22,
                    UserId = 2,
                    Ensemble_Name = "Cyrus Family Band",
                    Formed_Date = new System.DateTime(2006, 3, 1),
                    Type = "Family Band",
                    Genre = "Country",
                    City = "Flatwoods",
                    State = "Kentucky",
                    Pic_Url = "http://cdn.gospelherald.com/data/images/full/3611/miley-cyrus-and-billy-ray-cyrus.jpg"
                }
                );

            modelBuilder.Entity<Plays_Instrument>().HasData(
                new Plays_Instrument() { Id = 1, ProfileId = 11, InstrumentId = 2 },
                new Plays_Instrument() { Id = 2, ProfileId = 12, InstrumentId = 2 },
                new Plays_Instrument() { Id = 3, ProfileId = 12, InstrumentId = 1 },
                new Plays_Instrument() { Id = 4, ProfileId = 12, InstrumentId = 7 }
                );

            modelBuilder.Entity<ProfileEnsemble>().HasData(
                new ProfileEnsemble() { ProfileId = 11, EnsembleId = 21, Start_Date = new System.DateTime(2006, 3, 1) },
                new ProfileEnsemble() { ProfileId = 12, EnsembleId = 21, Start_Date = new System.DateTime(2006, 3, 1) },
                new ProfileEnsemble() { ProfileId = 11, EnsembleId = 22, Start_Date = new System.DateTime(2006, 3, 1) },
                new ProfileEnsemble() { ProfileId = 12, EnsembleId = 22, Start_Date = new System.DateTime(2006, 3, 1) }
                );

            modelBuilder.Entity<Post>().HasData(
                new Post()
                {
                    PostId = 1,
                    PosterType = "profile",
                    PosterIndex = 1,
                    MediaType = "img",
                    MediaUrl = "https://upload.wikimedia.org/wikipedia/en/0/06/Miley_Cyrus_-_Wrecking_Ball.jpg",
                    Text = "No longer a Disney gal!"
                },
                new Post() { PostId = 2, PosterType = "profile", PosterIndex = 11, Text = "Screw you dad @BillyRayCyrus" },
                new Post() { PostId = 3, PosterType = "profile", PosterIndex = 11, MediaType = "img", MediaUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a1/Miley_Cyrus_Gypsi_Tour_Acer_Arena_Sydney_%285872497845%29.jpg/255px-Miley_Cyrus_Gypsi_Tour_Acer_Arena_Sydney_%285872497845%29.jpg" },
                new Post() { PostId = 4, PosterType = "ensemble", PosterIndex = 21, Type = "aud", Ref_Id = 1, Text = "We're looking for a new member!" }
                );

            modelBuilder.Entity<Audition>().HasData(
                new Audition()
                {
                    AuditionId = 1,
                    Open_Date = new System.DateTime(2018, 12, 6),
                    Audition_Location = "Marty's Grill",
                    Audition_Description = "She's a diva and left us... We need a new Hannah Montana! (Not required to be from Montana)",
                    Instrument_Name = "Voice",
                    InstrumentId = 2,
                    EnsembleId = 21


                }
                );
        }
    }

    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
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
        public string Bio { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Plays_Instrument> Plays_Instrument { get; set; }
        public ICollection<ProfileEnsemble> ProfileEnsemble { get; set; }
        //POSTS
    }

    public class Ensemble
    {
        public int EnsembleId { get; set; }

        public string Ensemble_Name { get; set; }
        [Column(TypeName = "Date")]
        public System.DateTime Formed_Date { get; set; }
        [Column(TypeName = "Date")]
        public System.DateTime Disbanded_Date { get; set; }
        public string Type { get; set; }
        public string Genre { get; set; }
        public string Pic_Url { get; set; }
        public string Bio { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Booked_Gig> Booked_Gigs { get; set; }
        public ICollection<ProfileEnsemble> ProfileEnsemble { get; set; }
        public ICollection<Audition> Audition { get; set; }
        //POSTS
    }

    public class Venue
    {
        public int VenueId { get; set; }

        public string Venue_Name { get; set; }
        public string Pic_Url { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Bio { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        //phone, web, date formed

        public ICollection<Gig> Gig { get; set; }
        //POSTS
    }

    public class Instrument
    {
        public int InstrumentId { get; set; }
        public string Instrument_Name { get; set; }
    }

    public class Plays_Instrument
    {
        public int Id { get; set; }

        public int ProfileId { get; set; }
        public Profile Profile { get; set; }

        public int InstrumentId { get; set; }
        public Instrument Instrument { get; set; }
    }

    public class ProfileEnsemble
    {
        [Column(TypeName = "Date")]
        public System.DateTime Start_Date { get; set; }
        [Column(TypeName = "Date")]
        public System.DateTime End_Date { get; set; }

        public int ProfileId { get; set; }
        public Profile Profile { get; set; }

        public int EnsembleId { get; set; }
        public Ensemble Ensemble { get; set; }

        //POSTS
    }


    public class Audition
    {
        public int AuditionId { get; set; }
        public System.DateTime Open_Date { get; set; }
        public System.DateTime Closed_Date { get; set; }
        public string Audition_Location { get; set; }
        public string Audition_Description { get; set; }

        public string Instrument_Name { get; set; }

        public int InstrumentId { get; set; }
        public Instrument Instrument { get; set; }

        public int EnsembleId { get; set; }
        public Ensemble Ensemble { get; set; }

        //POSTS
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

        //POSTS
    }

    public class Post
    {
        public int PostId { get; set; }
        public string MediaType { get; set; }
        public string MediaUrl { get; set; }
        public string Text { get; set; }

        public string PosterType { get; set; }
        //Not technically a foreign key
        //We must manually join based on Poster Type
        //For example, if PosterType = "ensemble"
        //Then PosterIndex is actually an EnsembleId in disguise
        public int PosterIndex { get; set; }

        public string Type { get; set; }
        public int Ref_Id { get; set; }

        public Venue Venue { get; set; }
        public Booked_Gig Booked_Gig { get; set; }
        public Ensemble Ensemble { get; set; }
        public Audition Audition { get; set; }
        public ProfileEnsemble Membership { get; set; }
        public Profile Profile { get; set; }
    }

    /*
    public ICollection<P_Has_Media> P_Has_Media { get; set; }
    public ICollection<E_Has_Media> E_Has_Media { get; set; }
    public ICollection<V_Has_Media> V_Has_Media { get; set; }
}

public class P_Has_Media
{
    public int Id { get; set; }

    public int ProfileId { get; set; }
    public Profile Profile { get; set; }

    public int MediaId { get; set; }
    public PostMedia Media { get; set; }
}

public class E_Has_Media
{
    public int Id { get; set; }

    public int ProfileId { get; set; }
    public Profile Profile { get; set; }

    public int MediaId { get; set; }
    public PostMedia Media { get; set; }
}

public class V_Has_Media
{
    public int Id { get; set; }

    public int ProfileId { get; set; }
    public Profile Profile { get; set; }

    public int MediaId { get; set; }
    public PostMedia Media { get; set; }
}
*/


}