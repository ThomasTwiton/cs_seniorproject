using Moq;
using Xunit;
using System.Linq;
using server.Models;
using server.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PluggedIn_Tests
{
    public class TablesTest
    {
        [Fact]
        public void UserTable_WhenGivenValidData_StoresValidData()
        {
            /* Arrange */

            /* Act */
            User user = new User() { UserId = 1, Email = "test@email.com", Password = "test", Salt = "salt" };
            /* Assert */
            Assert.Equal(1, user.UserId);
            Assert.Equal("test@email.com", user.Email);
            Assert.Equal("test", user.Password);
            Assert.Equal("salt", user.Salt);
        }

        [Fact]
        public void ProfileTable_WhenGivenValidData_StoresValidData()
        {
            /* Arrange */

            /* Act */
            Profile profile = new Profile() { ProfileId = 11, UserId = 1,
                First_Name = "Test", Last_Name = "User",
                Pic_Url ="/uploads/image.png", Bio="I am a test user",
                City ="Chicago", State="IL" };
            /* Assert */
            Assert.Equal(11, profile.ProfileId);
            Assert.Equal(1, profile.UserId);
            Assert.Equal("Test", profile.First_Name);
            Assert.Equal("User", profile.Last_Name);
            Assert.Equal("/uploads/image.png", profile.Pic_Url);
            Assert.Equal("I am a test user", profile.Bio);
            Assert.Equal("Chicago", profile.City);
            Assert.Equal("IL", profile.State);
        }

        [Fact]
        public void EnsembleTable_WhenGivenValidData_StoresValidData()
        {
            /* Arrange */
            System.DateTime testdate = new System.DateTime(2019, 5, 2);

            /* Act */
            Ensemble ensemble = new Ensemble() { EnsembleId = 21, UserId = 1,
            Ensemble_Name = "Test Venue",
            Formed_Date = testdate,
            Type="Garage Band", Genre="Jazz",
            Pic_Url="/uploads/image.png", Bio="I am a test ensemble",
            City="Chicago", State="IL"};

            /* Assert */
            Assert.Equal(21, ensemble.EnsembleId);
            Assert.Equal(1, ensemble.UserId);
            Assert.Equal("Test Venue", ensemble.Ensemble_Name);
            Assert.Equal(testdate, ensemble.Formed_Date);
            Assert.Equal("Garage Band", ensemble.Type);
            Assert.Equal("Jazz", ensemble.Genre);
            Assert.Equal("/uploads/image.png", ensemble.Pic_Url);
            Assert.Equal("I am a test ensemble", ensemble.Bio);
            Assert.Equal("Chicago", ensemble.City);
            Assert.Equal("IL", ensemble.State);
        }

        [Fact]
        public void VenueTable_WhenGivenValidData_StoresValidData()
        {
            /* Arrange */

            /* Act */
            Venue venue = new Venue() { VenueId = 31, UserId =1,
            Venue_Name="Test Venue", Pic_Url="/uploads/image.png",
            Address1="13 Main Street", Address2="Apt 2",
            Bio="I am a test venue",
            City="Chicago", State="IL",
            Website="google.com", Phone="(123) 456-7890"};

            /* Assert */
            Assert.Equal(31, venue.VenueId);
            Assert.Equal(1, venue.UserId);
            Assert.Equal("Test Venue", venue.Venue_Name);
            Assert.Equal("/uploads/image.png", venue.Pic_Url);
            Assert.Equal("I am a test venue", venue.Bio);
            Assert.Equal("13 Main Street", venue.Address1);
            Assert.Equal("Apt 2", venue.Address2);
            Assert.Equal("Chicago", venue.City);
            Assert.Equal("IL", venue.State);
            Assert.Equal("google.com", venue.Website);
            Assert.Equal("(123) 456-7890", venue.Phone);
        }

        [Fact]
        public void InstrumentTable_WhenGivenValidData_StoresValidData()
        {
            /* Arrange */

            /* Act */
            Instrument instrument = new Instrument() { InstrumentId = 1, Instrument_Name = "Piano" };
            /* Assert */
            Assert.Equal(1, instrument.InstrumentId);
            Assert.Equal("Piano", instrument.Instrument_Name);
        }

        [Fact]
        public void Plays_InstrumentTable_WhenGivenValidData_StoresValidData()
        {
            /* Arrange */

            /* Act */
            Plays_Instrument pi = new Plays_Instrument() { Id=1, ProfileId=11, InstrumentId=1};
            /* Assert */
            Assert.Equal(1, pi.Id);
            Assert.Equal(1, pi.InstrumentId);
            Assert.Equal(11, pi.ProfileId);
        }

        [Fact]
        public void ProfileEnsembleTable_WhenGivenValidData_StoresValidData()
        {
            /* Arrange */
            System.DateTime startdate = new System.DateTime(2019, 5, 2);

            /* Act */
            ProfileEnsemble membership = new ProfileEnsemble() { Start_Date = startdate, ProfileId = 11, EnsembleId = 21 };

            /* Assert */
            Assert.Equal(startdate, membership.Start_Date);
            Assert.Equal(11, membership.ProfileId);
            Assert.Equal(21, membership.EnsembleId);
        }

        [Fact]
        public void AuditionTable_WhenGivenValidData_StoresValidData()
        {
            /* Arrange */
            System.DateTime opendate = new System.DateTime(2019, 5, 2);
            System.DateTime closedate = new System.DateTime(2020, 5, 2);

            /* Act */
            Audition audition = new Audition() { AuditionId =  1,
                Open_Date =opendate, Closed_Date=closedate,
                Audition_Location ="Marty's", Audition_Description = "A test audition",
                InstrumentId = 1, EnsembleId = 21
            };

            /* Assert */
            Assert.Equal(1, audition.AuditionId);
            Assert.Equal(1, audition.InstrumentId);
            Assert.Equal(21, audition.EnsembleId);
            Assert.Equal(opendate, audition.Open_Date);
            Assert.Equal(closedate, audition.Closed_Date);
            Assert.Equal("Marty's", audition.Audition_Location);
            Assert.Equal("A test audition", audition.Audition_Description);

        }

        [Fact]
        public void GigTable_WhenGivenValidData_StoresValidData()
        {
            /* Arrange */
            System.DateTime opendate = new System.DateTime(2019, 5, 2);
            System.DateTime closedate = new System.DateTime(2020, 5, 2);

            /* Act */
            Gig gig = new Gig() { GigId = 1, Gig_Date = opendate, Closed_Date=closedate,
                Genre = "Jazz", Description = "A test description",
                VenueId=31
            };

            /* Assert */
            Assert.Equal(1, gig.GigId);
            Assert.Equal(31, gig.VenueId);
            Assert.Equal(opendate, gig.Gig_Date);
            Assert.Equal(closedate, gig.Closed_Date);
            Assert.Equal("Jazz", gig.Genre);
            Assert.Equal("A test description", gig.Description);
        }

        [Fact]
        public void PostTable_WhenGivenValidData_StoresValidData()
        {
            /* Arrange */

            /* Act */
            Post post = new Post()
            {
                PostId = 1,
                MediaType = "img",
                MediaUrl = "/uploads/image.png",
                Text = "Test post",
                PosterType = "profile",
                PosterIndex = 11,
                Type = "aud",
                Ref_Id = 1
            };

            /* Assert */
            Assert.Equal(1, post.PostId);
            Assert.Equal("profile", post.PosterType);
            Assert.Equal(11, post.PosterIndex);
            Assert.Equal(1, post.Ref_Id);
            Assert.Equal("aud", post.Type);
            Assert.Equal("img", post.MediaType);
            Assert.Equal("/uploads/image.png", post.MediaUrl);
            Assert.Equal("Test post", post.Text);
        }

        [Fact]
        public void AuditionProfileTable_WhenGivenValidData_StoresValidData()
        {
            /* Arrange */

            /* Act */
            AuditionProfile ap = new AuditionProfile() { AuditionId = 1, ProfileId = 11 };

            /* Assert */
            Assert.Equal(1, ap.AuditionId);
            Assert.Equal(11, ap.ProfileId);

        }

        [Fact]
        public void GigApp_WhenGivenValidData_StoresValidData()
        {
            /* Arrange */

            /* Act */
            GigApp ga = new GigApp() { GigId = 1, ViewType = "profile", ViewId = 11 };

            /* Assert */
            Assert.Equal(1, ga.GigId);
            Assert.Equal("profile", ga.ViewType);
            Assert.Equal(11, ga.ViewId);
        }

        [Fact]
        public void BookedGig_WhenGivenValidData_StoresValidData()
        {
            /* Arrange */
            System.DateTime closedate = new System.DateTime(2020, 5, 2);

            /* Act */
            Booked_Gig bg = new Booked_Gig() { Id= 1, GigId = 1, Date_Booked= closedate };

            /* Assert */
            Assert.Equal(1, bg.GigId);
            Assert.Equal(closedate, bg.Date_Booked);
            Assert.Equal(1, bg.Id);
        }
    }
}
