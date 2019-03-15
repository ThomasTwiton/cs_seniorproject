using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Text;
using System.Security.Cryptography;

namespace server.Models
{
    public class PluggedContext : DbContext
    {
        private static string CreatePasswordHash(string pwd, string salt)
        {
            string saltAndPwd = pwd + salt;
            byte[] bytes = Encoding.ASCII.GetBytes(saltAndPwd);
            SHA512 shaM = new SHA512Managed();
            byte[] hashedPwdbytes = shaM.ComputeHash(bytes);
            string hashedPwd = Convert.ToBase64String(hashedPwdbytes);
            return hashedPwd;
        }

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
        public virtual DbSet<AuditionProfile> AuditionProfiles { get; set; }
        public virtual DbSet<GigApp> GigApps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<Profile>()
                .Property(p => p.Pic_Url).HasDefaultValue("/images/uploads/default.png");
            modelBuilder.Entity<Ensemble>()
                 .Property(p => p.Pic_Url).HasDefaultValue("/images/uploads/default.png");
            modelBuilder.Entity<Venue>()
                .Property(p => p.Pic_Url).HasDefaultValue("/images/uploads/default.png");

            modelBuilder.Entity<Gig>()
                .Property(g => g.Closed_Date).HasDefaultValue(System.DateTime.Now.AddMonths(1));
            modelBuilder.Entity<Audition>()
                .Property(g => g.Closed_Date).HasDefaultValue(System.DateTime.Now.AddMonths(1));
            modelBuilder.Entity<Ensemble>()
                .Property(e => e.Disbanded_Date).HasDefaultValue(new System.DateTime(9999, 12, 31));

            modelBuilder.Entity<ProfileEnsemble>()
                .HasKey(em => new { em.ProfileId, em.EnsembleId });

            modelBuilder.Entity<AuditionProfile>()
                .HasKey(em => new { em.AuditionId, em.ProfileId });

            modelBuilder.Entity<GigApp>()
                .HasKey(em => new { em.GigId, em.ViewId, em.ViewType });

            modelBuilder.Entity<ProfileEnsemble>()
                .HasOne(em => em.Profile)
                .WithMany(b => b.ProfileEnsemble)
                .HasForeignKey(em => em.ProfileId);

            modelBuilder.Entity<ProfileEnsemble>()
                .HasOne(em => em.Ensemble)
                .WithMany(c => c.ProfileEnsemble)
                .HasForeignKey(em => em.EnsembleId);

            modelBuilder.Entity<GigApp>()
                .HasOne(em => em.Gig)
                .WithMany(b => b.GigApp)
                .HasForeignKey(em => em.GigId);


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
                new User() { UserId = 1, Email = "tjtwiton@gmail.com", Password = CreatePasswordHash("faketom", "badpractice3"), Salt = "badpractice3" },
                new User() { UserId = 2, Email = "tyler@conzett.cmon", Password = CreatePasswordHash("bestRA", "unsafe3"), Salt = "unsafe3" }
            );
            modelBuilder.Entity<Profile>().HasData(
                new Profile()
                {
                    ProfileId = 11,
                    UserId = 1,
                    First_Name = "Thomas",
                    Last_Name = "Twiton",
                    Preferred_Name = "The Wrecking Ball",
                    Bio = "A senior at Luther College eager to get back into playing the piano",
                    Pic_Url = "/images/uploads/default.png",
                    City = "Los Angeles",
                    State = "Caliornia"
                },
                new Profile()
                {
                    ProfileId = 12,
                    UserId = 2,
                    First_Name = "Tyler",
                    Last_Name = "Conzett",
                    Pic_Url = "/images/uploads/default.png",
                    Preferred_Name = "Lord of the RAs",
                    Bio = "One RA to rule them all, and in the darkness bind them",
                    City = "Flatwoods",
                    State = "Kentucky"
                }
                );
            modelBuilder.Entity<Ensemble>().HasData(
                new Ensemble()
                {
                    EnsembleId = 21,
                    UserId = 2,
                    Ensemble_Name = "RA Show",
                    Formed_Date = new System.DateTime(2006, 3, 1),
                    Type = "TV Show",
                    Genre = "Pop",
                    Pic_Url = "/images/uploads/default.png",
                    City = "Disneyworld",
                    State = "Disney",
                    Bio = "Is it real?",
                },
                new Ensemble()
                {
                    EnsembleId = 22,
                    UserId = 1,
                    Ensemble_Name = "Sad Pianos",
                    Formed_Date = new System.DateTime(2006, 3, 1),
                    Type = "Cover Band",
                    Genre = "Alternative",
                    City = "Flatwoods",
                    State = "Kentucky",
                    Pic_Url = "/images/uploads/default.png"
                }
                );

            modelBuilder.Entity<Venue>().HasData(
                new Venue()
                {
                    VenueId = 31,
                    Venue_Name = "Marty's Grill",
                    Pic_Url = "https://www.luther.edu/reason/images/802151.jpg",
                    UserId = 1,
                    Address1 = "400 College Dr.",
                    Bio = "We do food.",
                    City = "Decorah",
                    State = "IA",
                    Website = "https://www.luther.edu/dining/locations/martys/",
                    Phone = "(563) 387-1395"
                }
                );

            modelBuilder.Entity<Gig>().HasData(
                new Gig()
                {
                    GigId = 41,
                    Gig_Date = new System.DateTime(2019, 12, 1),
                    Closed_Date = new System.DateTime(2006, 12, 30),
                    Genre = "Jazz",
                    Description = "We're looking for some live entertainment!",

                    VenueId = 31
                });

            modelBuilder.Entity<Audition>().HasData(
               new Audition()
               {
                   AuditionId = 1,
                   Open_Date = new System.DateTime(2018, 12, 6),
                   Audition_Location = "Marty's Grill",
                   Audition_Description = "We need RAs... Musical RAs!",
                   Instrument_Name = "Voice",
                   InstrumentId = 2,
                   EnsembleId = 21

               }
               );
            
            modelBuilder.Entity<AuditionProfile>().HasData(
                new AuditionProfile() { AuditionId = 1, ProfileId = 11 },
                new AuditionProfile() { AuditionId = 1, ProfileId = 12 }
                );

            modelBuilder.Entity<GigApp>().HasData(
                new GigApp { GigId = 41, ViewType = "profile", ViewId = 11 },
                new GigApp { GigId = 41, ViewType = "profile", ViewId = 12 },
                new GigApp { GigId = 41, ViewType = "ensemble", ViewId = 21 },
                new GigApp { GigId = 41, ViewType = "ensemble", ViewId = 22 }
                );

            modelBuilder.Entity<Plays_Instrument>().HasData(
                new Plays_Instrument() { Id = 1, ProfileId = 11, InstrumentId = 1 },
                new Plays_Instrument() { Id = 2, ProfileId = 11, InstrumentId = 10 },
                new Plays_Instrument() { Id = 3, ProfileId = 12, InstrumentId = 2 }
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
                    Text = "Hi everybody!"
                },
                new Post() { PostId = 2, PosterType = "ensemble", PosterIndex = 21, Type = "aud", Ref_Id = 1, Text = "We're looking for a new member!" }
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

        public string Salt { get; set; }

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

        public ICollection<AuditionProfile> AuditionProfile { get; set; }

        //POSTS
    }

    public class Gig
    {
        public int GigId { get; set; }
        public System.DateTime Gig_Date { get; set; }
        public System.DateTime Closed_Date { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }

        public int VenueId { get; set; }
        public Venue Venue { get; set; }

        public ICollection<GigApp> GigApp { get; set; }
        //posts
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
        public Gig Gig { get; set; }
        public Ensemble Ensemble { get; set; }
        public Audition Audition { get; set; }
        public ProfileEnsemble Membership { get; set; }
        public Profile Profile { get; set; }
    }

    public class AuditionProfile
    {
        public int AuditionId { get; set; }
        public Audition Audition { get; set; }
        public int ProfileId { get; set; }
        public Profile Profile { get; set; }
    }

    public class GigApp
    {
        public int GigId { get; set; }
        public Gig Gig { get; set; }
        public string ViewType { get; set; }
        public int ViewId { get; set; }
    }
}