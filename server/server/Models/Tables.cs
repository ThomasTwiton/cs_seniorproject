using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class PluggedContext : DbContext
    {

        public PluggedContext(DbContextOptions<PluggedContext> options)
            : base(options)
        {  }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

        }


        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Ensemble> Ensembles { get; set; }
        public DbSet<Instrument> Instruments{ get; set; }
        public DbSet<Plays_Instrument> Plays_Instruments { get; set; }
        public DbSet<ProfileEnsemble> ProfileEnsembles { get; set; }
        public DbSet<Audition> Auditoins { get; set; }
        public DbSet<Gig> Gigs { get; set; }
        public DbSet<Booked_Gig> Booked_Gigs { get; set; }
        public DbSet<PostMedia> PostMedias { get; set; }
        public DbSet<E_Has_Media> E_Has_Medias { get; set; }
        public DbSet<P_Has_Media> P_Has_Medias { get; set; }
        public DbSet<V_Has_Media> V_Has_Medias { get; set; }
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
        public ICollection<ProfileEnsemble> ProfileEnsemble { get; set; }
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

        public ICollection<ProfileEnsemble> ProfileEnsemble { get; set; }
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

        public int ProfileId { get; set; }
        public Profile Profile { get; set; }

        public int InstrumentId { get; set; }
        public Instrument Instrument { get; set; }
    }
 
    public class ProfileEnsemble
    {
        public System.DateTime Start_Date { get; set; }
        public System.DateTime End_Date { get; set; }

        public int ProfileId { get; set; }
        public Profile Profile { get; set; }
        
        public int EnsembleId { get; set; }
        public Ensemble Ensemble { get; set; }
    }
  

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

    public class PostMedia
    {
        public int PostMediaId { get; set; }
        public string MediaUrl { get; set; }

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

    

}
