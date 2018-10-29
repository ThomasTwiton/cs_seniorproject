﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using server.Models;

namespace server.Migrations
{
    [DbContext(typeof(PluggedContext))]
    partial class PluggedContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("server.Models.Audition", b =>
                {
                    b.Property<int>("AuditionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Audition_Description");

                    b.Property<string>("Audition_Location");

                    b.Property<DateTime>("Closed_Date");

                    b.Property<int>("EnsembleId");

                    b.Property<string>("Instrument_Name");

                    b.Property<DateTime>("Open_Date");

                    b.HasKey("AuditionId");

                    b.HasIndex("EnsembleId");

                    b.ToTable("Auditoins");
                });

            modelBuilder.Entity("server.Models.Booked_Gig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date_Booked");

                    b.Property<int>("GigId");

                    b.HasKey("Id");

                    b.HasIndex("GigId");

                    b.ToTable("Booked_Gigs");
                });

            modelBuilder.Entity("server.Models.E_Has_Media", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MediaId");

                    b.Property<int>("ProfileId");

                    b.HasKey("Id");

                    b.HasIndex("MediaId");

                    b.HasIndex("ProfileId");

                    b.ToTable("E_Has_Medias");
                });

            modelBuilder.Entity("server.Models.Ensemble", b =>
                {
                    b.Property<int>("EnsembleId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Disbanded_Date");

                    b.Property<string>("Ensemble_Name");

                    b.Property<DateTime>("Formed_Date");

                    b.Property<string>("Genre");

                    b.Property<string>("Pic_Url");

                    b.Property<string>("Type");

                    b.Property<int>("UserId");

                    b.HasKey("EnsembleId");

                    b.HasIndex("UserId");

                    b.ToTable("Ensembles");
                });

            modelBuilder.Entity("server.Models.Gig", b =>
                {
                    b.Property<int>("GigId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Gig_Date");

                    b.Property<int>("VenueId");

                    b.HasKey("GigId");

                    b.HasIndex("VenueId");

                    b.ToTable("Gigs");
                });

            modelBuilder.Entity("server.Models.Instrument", b =>
                {
                    b.Property<int>("InstrumentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Instrument_Name");

                    b.HasKey("InstrumentId");

                    b.ToTable("Instruments");
                });

            modelBuilder.Entity("server.Models.P_Has_Media", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MediaId");

                    b.Property<int>("ProfileId");

                    b.HasKey("Id");

                    b.HasIndex("MediaId");

                    b.HasIndex("ProfileId");

                    b.ToTable("P_Has_Medias");
                });

            modelBuilder.Entity("server.Models.Plays_Instrument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("InstrumentId");

                    b.Property<int>("ProfileId");

                    b.HasKey("Id");

                    b.HasIndex("InstrumentId");

                    b.HasIndex("ProfileId");

                    b.ToTable("Plays_Instruments");
                });

            modelBuilder.Entity("server.Models.PostMedia", b =>
                {
                    b.Property<int>("PostMediaId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("MediaUrl");

                    b.HasKey("PostMediaId");

                    b.ToTable("PostMedias");
                });

            modelBuilder.Entity("server.Models.Profile", b =>
                {
                    b.Property<int>("ProfileId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("First_Name");

                    b.Property<string>("Last_Name");

                    b.Property<string>("Pic_Url");

                    b.Property<string>("Preferred_Name");

                    b.Property<int>("UserId");

                    b.HasKey("ProfileId");

                    b.HasIndex("UserId");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("server.Models.ProfileEnsemble", b =>
                {
                    b.Property<int>("ProfileId");

                    b.Property<int>("EnsembleId");

                    b.Property<DateTime>("End_Date");

                    b.Property<DateTime>("Start_Date");

                    b.HasKey("ProfileId", "EnsembleId");

                    b.HasIndex("EnsembleId");

                    b.ToTable("ProfileEnsembles");
                });

            modelBuilder.Entity("server.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<string>("Password");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("server.Models.V_Has_Media", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MediaId");

                    b.Property<int>("ProfileId");

                    b.HasKey("Id");

                    b.HasIndex("MediaId");

                    b.HasIndex("ProfileId");

                    b.ToTable("V_Has_Medias");
                });

            modelBuilder.Entity("server.Models.Venue", b =>
                {
                    b.Property<int>("VenueId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Location");

                    b.Property<int>("UserId");

                    b.Property<string>("Venue_Name");

                    b.HasKey("VenueId");

                    b.HasIndex("UserId");

                    b.ToTable("Venues");
                });

            modelBuilder.Entity("server.Models.Audition", b =>
                {
                    b.HasOne("server.Models.Ensemble", "Ensemble")
                        .WithMany("Audition")
                        .HasForeignKey("EnsembleId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("server.Models.Booked_Gig", b =>
                {
                    b.HasOne("server.Models.Gig", "Gig")
                        .WithMany("Booked_Gig")
                        .HasForeignKey("GigId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("server.Models.E_Has_Media", b =>
                {
                    b.HasOne("server.Models.PostMedia", "Media")
                        .WithMany("E_Has_Media")
                        .HasForeignKey("MediaId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("server.Models.Profile", "Profile")
                        .WithMany()
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("server.Models.Ensemble", b =>
                {
                    b.HasOne("server.Models.User", "User")
                        .WithMany("Ensemble")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("server.Models.Gig", b =>
                {
                    b.HasOne("server.Models.Venue", "Venue")
                        .WithMany("Gig")
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("server.Models.P_Has_Media", b =>
                {
                    b.HasOne("server.Models.PostMedia", "Media")
                        .WithMany("P_Has_Media")
                        .HasForeignKey("MediaId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("server.Models.Profile", "Profile")
                        .WithMany()
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("server.Models.Plays_Instrument", b =>
                {
                    b.HasOne("server.Models.Instrument", "Instrument")
                        .WithMany("Plays_Instrument")
                        .HasForeignKey("InstrumentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("server.Models.Profile", "Profile")
                        .WithMany("Plays_Instrument")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("server.Models.Profile", b =>
                {
                    b.HasOne("server.Models.User", "User")
                        .WithMany("Profile")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("server.Models.ProfileEnsemble", b =>
                {
                    b.HasOne("server.Models.Ensemble", "Ensemble")
                        .WithMany("ProfileEnsemble")
                        .HasForeignKey("EnsembleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("server.Models.Profile", "Profile")
                        .WithMany("ProfileEnsemble")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("server.Models.V_Has_Media", b =>
                {
                    b.HasOne("server.Models.PostMedia", "Media")
                        .WithMany("V_Has_Media")
                        .HasForeignKey("MediaId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("server.Models.Profile", "Profile")
                        .WithMany()
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("server.Models.Venue", b =>
                {
                    b.HasOne("server.Models.User", "User")
                        .WithMany("Venue")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
